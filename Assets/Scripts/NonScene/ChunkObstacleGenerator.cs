using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkObstacleGenerator : ObjectGenerator<PrefabInfo>
{
    [Header("Chunk settings")]
    [SerializeField] private float _chunkHalfSize = 50;
    [Range(10, 100)]
    [SerializeField] private int _maxCheckAreaAttempts = 50;
    [Range(0, 10)]
    [SerializeField] private int _minObstacleCount = 6;
    [Range(0, 50)]
    [SerializeField] private int _maxObstacleCount = 14;
    private int _obstacleCount;

    [Header("Prefabs settings")]
    [SerializeField] private List<PrefabInfo> _prefabsInfo;
    private List<CachedInfo> _cachedInfo;

    [Header("Debug perlin noise")]
    [SerializeField] private bool _enableDebugTexture = false;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private int _width = 256;
    [SerializeField] private int _height = 256;

    private bool beforeStart = true;
    private ChunkBounds _chunkBounds;
    private class ChunkBounds
    {
        public ChunkBounds(Vector3 position, float chunkHalfSize)
        {
            _left = position.x - chunkHalfSize;
            _right = position.x + chunkHalfSize;
            _up = position.z + chunkHalfSize;
            _bottom = position.z - chunkHalfSize;
        }
        private readonly float _left, _right, _up, _bottom;
        public float Left => _left;
        public float Right => _right;
        public float Up => _up;
        public float Bottom => _bottom;
    }
    private class CachedInfo
    {
        private readonly int _id;
        private GameObject _inst;
        private Vector3 _position;
        private Quaternion _rotation;

        public int Id => _id;
        public GameObject Inst => _inst;
        public CachedInfo(int id, Vector3 position, Quaternion rotation)
        {
            _id = id;
            _position = position;
            _rotation = rotation;
        }
        public void UpdateData(GameObject inst, Transform parent)
        {
            _inst = inst;
            inst.transform.position = _position;
            inst.transform.rotation = _rotation;
            inst.transform.SetParent(parent);
            inst.SetActive(true);
        }
        public void SetData(GameObject prefab, Transform parent)
        {
            _inst = Instantiate(prefab, _position, _rotation);
            _inst.transform.SetParent(parent);
        }
        // add object state (because same objects may be in different chunks)
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        if (beforeStart) beforeStart = false;
        else
        {
            GetAllObstaclesFromPool();
            if(PhotonNetwork.IsMasterClient) ChunkGenerator.AddChange();
        }
    }
    private void OnDisable() 
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        AddAllObstaclesToPool();
        if (PhotonNetwork.IsMasterClient) ChunkGenerator.AddChange();
    }

    private void Awake()
    {
        SetNoiseOffsets();
        _cachedInfo = new List<CachedInfo>();
        if (!PhotonNetwork.IsMasterClient) return;
        _chunkBounds = new ChunkBounds(transform.position, _chunkHalfSize);
        _obstacleCount = Random.Range(_minObstacleCount, _maxObstacleCount);

        if(_enableDebugTexture) _renderer.material.mainTexture = DebugOnlyGenerateTexture();
    }

    private void Start()
    {
        StartCoroutine(SetSpawnDataRoutine());
    }

    #region PunEvent Methods
    private void RaiseEventSpawnObstacles(List<int> ids, List<object> positions, List<object> rotations)
    {
        object[] data = new object[] {
            Constants.PunEvent.SpawnObstacles,
            transform.position,
            ids.ToArray(),
            positions.ToArray(),
            rotations.ToArray() };

        PhotonNetwork.RaiseEvent(
            Constants.PunEvent.Code,
            data,
            new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable);
    }
    private void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != Constants.PunEvent.Code) return;

        object[] data = (object[])photonEvent.CustomData;
        int eventType = (int)data[0];

        switch (eventType)
        {
            case Constants.PunEvent.SpawnObstacles:
                {
                    Vector3 senderPos = (Vector3)data[1];  // позиция чанка, отправившего ивент. Это гарантирует, что чанк примет ток один ивент для себя
                    if (senderPos != transform.position) return;

                    int[] id = (int[])data[2];
                    List<Vector3> positions = new List<Vector3>();
                    List<Quaternion> rotations = new List<Quaternion>();
                    foreach(object position in (object[])data[3])
                    {
                        positions.Add((Vector3)position);
                    } 
                    foreach(object rotation in (object[])data[4])
                    {
                        rotations.Add((Quaternion)rotation);
                    }

                    StartCoroutine(SpawnObstaclesRoutine(id, positions.ToArray(), rotations.ToArray()));
                }
                break;
        }
    }
    #endregion

    #region Spawn Obstacles Methods
    private IEnumerator SetSpawnDataRoutine()
    {
        List<int> ids = new List<int>();
        List<object> positions = new List<object>();
        List<object> rotations = new List<object>();

        int id;
        Vector3 position;
        Quaternion rotation;

        for (int i = 0; i < _obstacleCount; i++)
        {
            if (OverlapArea(out id, out position, out rotation)) continue;

            ids.Add(id);
            positions.Add(position);
            rotations.Add(rotation);

            yield return StartCoroutine(SpawnObstaclesRoutine(new int[] { id }, new Vector3[] { position }, new Quaternion[] { rotation }));
            yield return null;
        }
        ChunkGenerator.AddChange();
        RaiseEventSpawnObstacles(ids, positions, rotations);
       
    }
    private IEnumerator SpawnObstaclesRoutine(int[] id, Vector3[] positions, Quaternion[] rotations)
    {
        for (int i = 0; i < id.Length; i++)
        {
            CachedInfo info = new CachedInfo(id[i], positions[i], rotations[i]);
            GameObject inst = ObjectPool.Get(info.Id);

            if (inst != null) info.UpdateData(inst, transform);
            else info.SetData(GetById(_prefabsInfo, info.Id).Prefab, transform);
            _cachedInfo.Add(info);
            yield return null;
        }
    }
    private bool OverlapArea(out int id, out Vector3 position, out Quaternion rotation)
    {
        id = 0;
        position = Vector3.zero;
        rotation = GetRandomRotation();

        for (int i = 0; i < _maxCheckAreaAttempts; i++)
        {
            position = GetRandomPoint(_chunkBounds.Left, _chunkBounds.Right, _chunkBounds.Bottom, _chunkBounds.Up);
            PrefabInfo info = GetByPerlinNoiseAtPosition(_prefabsInfo, position);

            if (info == null) continue;
            if (info.Prefab.GetComponent<ChunkObstacle>().Overlapping(position, rotation)) continue;
            else
            {
                id = info.Id;
                return false;
            }
        }
        return true;
    }
    #endregion

    #region Object Pool Interaction Methods
    private void AddAllObstaclesToPool()
    {
        Dictionary<GameObject, int> objectsToPool = new Dictionary<GameObject, int>();
        foreach (CachedInfo info in _cachedInfo)
        {
            objectsToPool.Add(info.Inst, info.Id);
        }
        ObjectPool.Add(objectsToPool);
    }
    private void GetAllObstaclesFromPool()
    {
        foreach (CachedInfo info in _cachedInfo)
        {
            GameObject inst = ObjectPool.Get(info.Id);
            if (inst != null) info.UpdateData(inst, transform);
            else info.SetData(GetById(_prefabsInfo, info.Id).Prefab, transform);
        }
    }
    #endregion

    #region debug Texture Perlin Noise
    private Texture2D DebugOnlyGenerateTexture()
    {
        Texture2D texture = new Texture2D(_width, _height);
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Color color = OnlyDebugCalculateColor(x, y, _width, _height, _chunkHalfSize*2, transform.position);
                texture.SetPixel(x, y, color);
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        return texture;
    }
    #endregion
}

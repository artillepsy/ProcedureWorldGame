using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChunkGenerator : ObjectGenerator<PrefabInfo>, IOnEventCallback, IInRoomCallbacks
{
    const int TRUE_CODE = -1;
    const int FALSE_CODE = -2;

    private static int _changesCount = 0;
    private static int _lastChangesCount = 0;
    public static UnityEvent OnChangesEnd = new UnityEvent();

    [Header("Global parent")]
    [SerializeField] private Transform _environement;

    [Header("Check settings")]
    [SerializeField] private float _chunkLength = 100; // standart plane mesh with 10 units size
    [SerializeField] private int _checkStepsToEnableChunk = 1; // left bottom start chunk to check
    [SerializeField] private int _checkStepsToDisableChunk = 2;
    [SerializeField] private float _checkRepeatSeconds = 3;

    [Header("Prefabs settings")]
    [SerializeField] private List<PrefabInfo> _prefabsInfo;

    public int CheckMatrixSize => (_checkStepsToEnableChunk * 2 + 1);
    public float —hunkLength => _chunkLength;
    public float —heckRepeatSeconds => _checkRepeatSeconds;

    private float _checkSteps;
    private float _minMidCheckSteps;
    private float _maxMidCheckSteps;
    private Dictionary<Vector3, GameObject> _spawnedChunks;
    private List<Transform> _playersTransforms;
    private Dictionary<object, int> _spawnData;

    private void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    private void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);
    private void Awake()
    {
        SetNoiseOffsets();

        _playersTransforms = new List<Transform>();
        _spawnedChunks = new Dictionary<Vector3, GameObject>();
        _spawnData = new Dictionary<object, int>();

        _checkSteps = _checkStepsToDisableChunk * 2 + 1;
        _minMidCheckSteps = _checkStepsToDisableChunk - _checkStepsToEnableChunk;
        _maxMidCheckSteps = _checkStepsToDisableChunk + _checkStepsToEnableChunk;
    }

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        StartCoroutine(GenerateChunksRoutine());
    }

    #region Changes count Methods
    private static void SetChangesCount(int count) 
    {
        _lastChangesCount = count;
        _changesCount = 0; 
    }
    public static void AddChange()
    {
        _changesCount++;
        if(_changesCount >= _lastChangesCount)
        {
            OnChangesEnd?.Invoke();
            _lastChangesCount = 0;
        }
    }
    #endregion

    #region Chunk Generator Methods
   
    private IEnumerator GenerateChunksRoutine()
    {
        yield return StartCoroutine(FindPlayersRoutine());
        
        while (true)
        {
            foreach (Transform plrTransform in _playersTransforms)
            {
                CheckCenterPositions(plrTransform.position);
            }

            PrepareData();
            SetChangesCount(_spawnData.Count);
            RaiseEvent(Constants.PunEvent.SpawnChunks);
            _spawnData.Clear();

            yield return new WaitForSeconds(_checkRepeatSeconds);
        }
    }

    // check matrix from left button
    private void CheckCenterPositions(Vector3 playerPosition)
    {
        int i, j;
        float x, z;
        Vector3 start = GetStart(playerPosition);
        Dictionary<object, int> buffSpawnData = new Dictionary<object, int>();

        for (x = start.x, i = 0; i < _checkSteps; i++, x = start.x + _chunkLength * i)
        {
            for (z = start.z, j = 0; j < _checkSteps; j++, z = start.z + _chunkLength * j)
            {
                Vector3 checkPoint = new Vector3(x, 0, z);

                if (Middle(i, j))
                {
                    if(_spawnedChunks.ContainsKey(checkPoint))
                    {
                        //if(!_spawnedChunks[checkPoint].activeSelf)
                            buffSpawnData.Add(checkPoint, TRUE_CODE);
                    }
                    else
                    {
                        PrefabInfo info = GetByPerlinNoiseAtPosition(_prefabsInfo, checkPoint, false);
                        buffSpawnData.Add(checkPoint, info.Id);
                    }
                }
                else if (_spawnedChunks.ContainsKey(checkPoint))
                {
                    //if (_spawnedChunks[checkPoint].activeSelf)
                        buffSpawnData.Add(checkPoint, FALSE_CODE);
                }
            }
        }
        RemoveUnnesessaryData(buffSpawnData);
    }

    #region additional Chunk check methods
    private Vector3 GetStart(Vector3 pos)
    {
        float centerX = Mathf.RoundToInt((pos.x - _checkStepsToDisableChunk * _chunkLength) / _chunkLength) * _chunkLength;
        float centerZ = Mathf.RoundToInt((pos.z - _checkStepsToDisableChunk * _chunkLength) / _chunkLength) * _chunkLength;
        return new Vector3(centerX, 0, centerZ);
    }
    private bool Middle(int i, int j) => (i >= _minMidCheckSteps && i <= _maxMidCheckSteps && j >= _minMidCheckSteps && j <= _maxMidCheckSteps);
    private void RemoveUnnesessaryData(Dictionary<object, int> spawnData)
    {
        foreach(KeyValuePair<object, int> data in spawnData)
        {
            if(!_spawnData.ContainsKey(data.Key))
            {
                _spawnData.Add(data.Key, data.Value);
                continue;
            }
            switch(data.Value)
            {
                case TRUE_CODE:
                    if (_spawnData[data.Key] == FALSE_CODE) _spawnData.Remove(data.Key);
                    break;
                case FALSE_CODE:
                    break;
                default:
                    break;
            }
        }
    }
    private void PrepareData()
    {
        Dictionary<object, int> buffer = new Dictionary<object, int>(_spawnData);
        foreach(KeyValuePair<object, int> data in buffer)
        {
            switch (data.Value)
            {
                case FALSE_CODE:
                    if (_spawnedChunks[(Vector3)data.Key].activeSelf == false)
                        _spawnData.Remove(data.Key);
                    break;
                case TRUE_CODE:
                    if (_spawnedChunks[(Vector3)data.Key].activeSelf == true)
                        _spawnData.Remove(data.Key);
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #endregion

    #region Event Methods
    private void RaiseEvent(int eventType)
    {
        switch (eventType)
        {
            case Constants.PunEvent.SpawnChunks:
                if (_spawnData.Count > 0)
                {
                    object[] content = new object[] {
                    Constants.PunEvent.SpawnChunks,
                    _spawnData};

                    PhotonNetwork.RaiseEvent(
                        Constants.PunEvent.Code,
                        content,
                        new RaiseEventOptions { Receivers = ReceiverGroup.All },
                        SendOptions.SendReliable);
                }
                break;
        }
    }
    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        if (!photonEvent.Code.Equals(Constants.PunEvent.Code)) return;

        object[] data = (object[])photonEvent.CustomData;
        int eventType = (int)data[0];

        switch (eventType)
        {
            case Constants.PunEvent.SpawnChunks:
                Dictionary<Vector3, int> spawnData = new Dictionary<Vector3, int>();

                foreach (KeyValuePair<object, int> info in (Dictionary<object, int>)data[1])
                {
                    spawnData.Add((Vector3)info.Key, info.Value);
                }
                ReformTerrain(spawnData);
                break;
        }
    }
    private void ReformTerrain(Dictionary<Vector3, int> spawnData)
    {
        foreach (KeyValuePair<Vector3, int> data in spawnData)
        {
            switch(data.Value)
            {
                case FALSE_CODE:
                    SetActiveChunk(data.Key, false);
                    break;
                case TRUE_CODE:
                    SetActiveChunk(data.Key, true);
                    break;
                default:
                    SpawnChunk(data.Key, data.Value);
                    break;
            }
        }
    }
    private void SpawnChunk(Vector3 position, int id)
    {
        GameObject chunk = Instantiate(GetById(_prefabsInfo, id).Prefab, position, Quaternion.identity);
        if (chunk == null) Debug.LogError("null reference to chunk gameObject");
        chunk.transform.SetParent(_environement.transform);
        _spawnedChunks.Add(position, chunk);
    }
    private void SetActiveChunk(Vector3 position, bool status)
    {
        GameObject chunk;
        if (!_spawnedChunks.TryGetValue(position, out chunk))
        {
            Debug.LogError("Spawned chunk doesn't exist");
        }
        else chunk.SetActive(status);
    }
    #endregion

    #region Players methods
    private IEnumerator FindPlayersRoutine()
    {
        while (true)
        {
            PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
            if (players.Length == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                foreach (PlayerMovement player in players)
                {
                    _playersTransforms.Add(player.transform);
                }
                yield break;
            }
            yield return null;
        }
    }
    void IInRoomCallbacks.OnPlayerLeftRoom(Player otherPlayer) //‰Ó‰ÂÎ‡Ú¸
    {
        foreach (Transform player in _playersTransforms)
        {
            if (player.GetComponent<PhotonView>().OwnerActorNr.Equals(otherPlayer.ActorNumber))
            {
                _playersTransforms.Remove(player);
                Debug.Log("Player left room. Player count: " + _playersTransforms.Count);
                return;
            }
        }
    }
    #region Unused
    public void OnPlayerEnteredRoom(Player newPlayer) => throw new System.NotSupportedException();
    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) { }
    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps) => throw new System.NotSupportedException();
    public void OnMasterClientSwitched(Player newMasterClient) => throw new System.NotSupportedException();
    #endregion
    #endregion
}
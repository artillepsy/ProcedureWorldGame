using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Photon.Pun;
using Photon.Realtime;

public class GridMover : MonoBehaviourPunCallbacks
{
    [SerializeField] private ChunkGenerator _chunkGenerator;

    [Header("Grid Settings")]
    [SerializeField] private float _nodeSize = 1;
    [SerializeField] private float _checkCollisionSphereDiameter = 1.5f;

    private List<StoredInfo> _storedInfos;
    private float _chunkLength;
    private int _nodeWidth;
    private bool _isScanning = false;

    private class StoredInfo
    {
        public LayerGridGraph graph;
        public Transform playerTransform;
        public Vector3 lastPosition = Vector3.negativeInfinity;
        public StoredInfo(Transform playerTransform)
        {
            this.playerTransform = playerTransform;
        }
        public bool UpdatePosition(Vector3 position)
        {
            if(position != lastPosition)
            {
                graph.center = position;
                lastPosition = position;
                return true;
            }
            else return false;
        }
        public void CreateGraph(int width, float nodeSize, float checkDiameter)
        {
            if (nodeSize == 0) Debug.LogError("division by zero");
            graph = AstarPath.active.data.AddGraph(typeof(LayerGridGraph)) as LayerGridGraph;
            graph.SetDimensions(width, width, nodeSize);
            graph.collision.heightCheck = false;
            graph.collision.type = ColliderType.Sphere;
            graph.collision.diameter = checkDiameter;
            graph.collision.mask = Constants.LayerMasks.Obstacle;
        }
    }

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient && !PhotonNetwork.OfflineMode)
        {
            enabled = false;
            return;
        }
        _storedInfos = new List<StoredInfo>();
        ChunkGenerator.OnChangesEnd.AddListener(MoveGrids);
    }
    void Start()
    {
        _chunkLength = _chunkGenerator.ÑhunkLength;
        _nodeWidth = Mathf.RoundToInt(_chunkLength / _nodeSize) * _chunkGenerator.CheckMatrixSize;
        StartCoroutine(SetUpRoutine());
    }
    private IEnumerator ScanGraphsRoutine()
    {
        if (_isScanning) yield return null;

        _isScanning = true;

        foreach (var progress in AstarPath.active.ScanAsync())
        {
            yield return null;
        }
        _isScanning = false;
    }
    private Vector3 ConvertPlayerPosition(Vector3 position)
    {
        float x = Mathf.RoundToInt(position.x / _chunkLength) * _chunkLength;
        float z = Mathf.RoundToInt(position.z / _chunkLength) * _chunkLength;
        return new Vector3(x, 0, z);
    }
    private void MoveGrids()
    {
        foreach (StoredInfo info in _storedInfos)
        {
            Vector3 position = ConvertPlayerPosition(info.playerTransform.position);
            info.UpdatePosition(position);
        }
        StartCoroutine(ScanGraphsRoutine());
    }
    private IEnumerator SetUpRoutine()
    {
        yield return StartCoroutine(FindPlayersRoutine());
        SpawnGrids();
    }
    private IEnumerator FindPlayersRoutine()
    {
        while (true)
        {
            PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
            if (players.Length == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                foreach (PlayerMovement player in players)
                {
                    _storedInfos.Add(new StoredInfo(player.transform));
                }
                yield break;
            }
            yield return null;
        }
    }
    private void SpawnGrids()
    {
        foreach(StoredInfo info in _storedInfos)
        {
            info.CreateGraph(_nodeWidth, _nodeSize, _checkCollisionSphereDiameter);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        foreach (PlayerMovement player in FindObjectsOfType<PlayerMovement>())
        {
            if (player.GetComponent<PhotonView>().OwnerActorNr == otherPlayer.ActorNumber)
            {
                foreach (StoredInfo info in _storedInfos)
                {
                    if (info.playerTransform == player.transform)
                    {
                        _storedInfos.Remove(info);
                        break;
                    }
                }
            }
        }
    }
}

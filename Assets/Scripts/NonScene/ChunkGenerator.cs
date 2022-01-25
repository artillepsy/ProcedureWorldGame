using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : ObjectGenerator<PrefabInfo>, IOnEventCallback, IInRoomCallbacks
{
    [SerializeField] private Transform _environement;

    [Header("Check settings")]
    [SerializeField] private float _chunkLength = 100; // standart plane mesh with 10 units size
    [SerializeField] private int _checkStepsToEnableChunk = 1; // left bottom start chunk to check
    [SerializeField] private int _checkStepsToDisableChunk = 2;
    [SerializeField] private float _checkRepeatSeconds = 3;
    public int CheckMatrixSize => (_checkStepsToDisableChunk * 2 + 1);
    public float —hunkLength => _chunkLength;
    public float —heckRepeatSeconds => _checkRepeatSeconds;


    [Header("Prefabs settings")]
    [SerializeField] private List<PrefabInfo> _prefabsInfo;

    private float _checkSteps;
    private float _minMidCheckSteps;
    private float _maxMidCheckSteps;

    private Dictionary<Vector3, GameObject> _spawnedChunks;
    private List<Transform> _playersTransforms;

    private Dictionary<object, int> _chunksToSpawn;
    private Dictionary<object, bool> _chunksToChangeStatus;

    private void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    private void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);
    private void Awake()
    {
        SetNoiseOffsets();

        _playersTransforms = new List<Transform>();
        _spawnedChunks = new Dictionary<Vector3, GameObject>();

        _chunksToSpawn = new Dictionary<object, int>();
        _chunksToChangeStatus = new Dictionary<object, bool>();

        _checkSteps = _checkStepsToDisableChunk * 2 + 1;
        _minMidCheckSteps = _checkStepsToDisableChunk - _checkStepsToEnableChunk;
        _maxMidCheckSteps = _checkStepsToDisableChunk + _checkStepsToEnableChunk;

        if (!PhotonNetwork.IsMasterClient) return;
        
    }
    private void Start()
    {
        StartCoroutine(GenerateChunksRoutine());
    }

    #region Chunk Generator Methods
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
    private IEnumerator GenerateChunksRoutine()
    {
        yield return StartCoroutine(FindPlayersRoutine());

        while (true)
        {
            _chunksToSpawn.Clear();
            _chunksToChangeStatus.Clear();

            foreach (Transform plrTransform in _playersTransforms)
            {
                CheckCenterPositions(plrTransform.position);
            }

            RaiseEvent(Constants.PunEvent.SpawnChunks);

            yield return new WaitForSeconds(_checkRepeatSeconds);
        }
    }

    private void CheckCenterPositions(Vector3 playerPosition)
    {
        float chunkCenterX = Mathf.RoundToInt((playerPosition.x - _checkStepsToDisableChunk * _chunkLength) / _chunkLength) * _chunkLength;
        float chunkCenterZ = Mathf.RoundToInt((playerPosition.z - _checkStepsToDisableChunk * _chunkLength) / _chunkLength) * _chunkLength;
        int i, j;
        float x, z;

        for (x = chunkCenterX, i = 0; i < _checkSteps; i++, x = chunkCenterX + _chunkLength * i)
        {
            for (z = chunkCenterZ, j = 0; j < _checkSteps; j++, z = chunkCenterZ + _chunkLength * j)
            {
                Vector3 checkPoint = new Vector3(x, 0, z);

                if (i >= _minMidCheckSteps && i <= _maxMidCheckSteps && j >= _minMidCheckSteps && j <= _maxMidCheckSteps)
                {
                    CheckChunkToAdd(checkPoint);
                }
                else
                {
                    CheckChunkToRemove(checkPoint);
                }
            }
        }
    }

    private void CheckChunkToAdd(Vector3 checkPoint)
    {
        if (_spawnedChunks.ContainsKey(checkPoint))
        {
            //Debug.DrawLine(checkPoint, checkPoint + Vector3.up * 3, Color.black, 20)
            if (_chunksToChangeStatus.ContainsKey(checkPoint))
            {
                if (_chunksToChangeStatus[checkPoint] == false)
                {
                    _chunksToChangeStatus.Remove(checkPoint);
                    _chunksToChangeStatus.Add(checkPoint, true);
                }
            }
            else _chunksToChangeStatus.Add(checkPoint, true);
        }
        else
        {
            // Debug.DrawLine(checkPoint, checkPoint + Vector3.up * 3, Color.green, 20)
            PrefabInfo info = GetByPerlinNoiseAtPosition(_prefabsInfo, checkPoint, false);
            if (!_chunksToSpawn.ContainsKey(checkPoint))
            {
                _chunksToSpawn.Add(checkPoint, info.Id);
            }
        }
    }
    private void CheckChunkToRemove(Vector3 checkPoint)
    {
        if (_spawnedChunks.ContainsKey(checkPoint) && !_chunksToChangeStatus.ContainsKey(checkPoint))
        {
            //Debug.DrawLine(checkPoint, checkPoint + Vector3.up * 30, Color.red, 220)
            _chunksToChangeStatus.Add(checkPoint, false);
        }
    }
    #endregion
    
    #region PunEvent
    private void RaiseEvent(int eventType)
    {
        switch (eventType)
        {
            case Constants.PunEvent.SpawnChunks:
                if (_chunksToSpawn.Count > 0 || _chunksToChangeStatus.Count > 0)
                {
                    object[] content = new object[] {
                    Constants.PunEvent.SpawnChunks,
                    _chunksToSpawn,
                    _chunksToChangeStatus };

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
                {
                    Dictionary<Vector3, int> spawnInfo = new Dictionary<Vector3, int>();
                    Dictionary<Vector3, bool> statusInfo = new Dictionary<Vector3, bool>();

                    foreach (KeyValuePair<object, int> info in (Dictionary<object, int>)data[1])
                    {
                        spawnInfo.Add((Vector3)info.Key, info.Value);
                    }
                    foreach(KeyValuePair<object, bool> info in (Dictionary<object, bool>)data[2])
                    {
                        statusInfo.Add((Vector3)info.Key, info.Value);
                    }

                    SpawnChunks(spawnInfo);
                    ChangeChunksStatus(statusInfo);
                }
                break;
        }
    }
    private void SpawnChunks(Dictionary<Vector3, int> spawnData)
    {
        foreach (KeyValuePair<Vector3, int> data in spawnData)
        {
            GameObject chunk = Instantiate(GetById(_prefabsInfo, data.Value).Prefab, data.Key, Quaternion.identity);
            if (chunk == null) Debug.LogError("null reference to chunk gameObject");
            chunk.transform.SetParent(_environement.transform);
            _spawnedChunks.Add(data.Key, chunk);
        }
    }
    private void ChangeChunksStatus(Dictionary<Vector3, bool> statusData)
    {
        foreach (KeyValuePair<Vector3, bool> data in statusData)
        {
            GameObject chunk;
            if (!_spawnedChunks.TryGetValue(data.Key, out chunk))
            {
                Debug.LogError("Spawned chunk doesn't exist");
            }
            else chunk.SetActive(data.Value);
        }
    }
    #endregion

    #region Implement pun methods
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
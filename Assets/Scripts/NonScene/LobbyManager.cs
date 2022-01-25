using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private LobbyCanvasManager _canvasManager;
    [SerializeField] private string _levelName = "Test";
    [SerializeField] private string _playerNickName = "Player";
    private readonly List<RoomInfo> _cachedRoomInfo = new List<RoomInfo>();

    void Start()
    {
        ChangeNickname(_playerNickName);
        Connect();
    }

    #region My Methods
    public void Connect()
    {
        if (PhotonNetwork.IsConnected) return;
        _canvasManager.ShowCanvas(_canvasManager.ConnectionCanvas);
        PhotonNetwork.ConnectUsingSettings();
    }
    public void ChangeNickname(string nickName) => PhotonNetwork.NickName = nickName;
    public void CreateRoom(string roomName)
    {
        if (!PhotonNetwork.IsConnectedAndReady) return;

        RoomOptions options = new RoomOptions
        {
            PlayerTtl = 60000,
            EmptyRoomTtl = 1000,
            MaxPlayers = 4,
            IsOpen = true,
        };
        PhotonNetwork.CreateRoom(roomName, options);
    }
    public void LeaveRoom()
    {
        if (!PhotonNetwork.IsConnectedAndReady) return;
        PhotonNetwork.LeaveRoom(false);
    }
    public void JoinRoom(string roomName)
    {
        if (!PhotonNetwork.IsConnectedAndReady) return;
        PhotonNetwork.JoinRoom(roomName);
    }
    public void UpdatePlayerList()
    {
        List<string> nickNames = new List<string>();
        foreach (Player plr in PhotonNetwork.PlayerList)
        {
            nickNames.Add(plr.NickName);
        }
        _canvasManager.OnPlayerListUpdate(nickNames);

        if (PhotonNetwork.CurrentRoom.MasterClientId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            _canvasManager.SetRootRules(true);
        }
        else
        {
            _canvasManager.SetRootRules(false);
        }
    }
    public void LoadGame() => PhotonNetwork.LoadLevel(_levelName);
    #endregion

    #region Override photon methods
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby(); 
    public override void OnJoinedLobby() => _canvasManager.ShowCanvas(_canvasManager.LobbyCanvas); 
    public override void OnDisconnected(DisconnectCause cause) => _canvasManager.ShowCanvas(_canvasManager.LostConnectionCanvas); 
    public override void OnCreatedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        UpdatePlayerList();
        _canvasManager.SetRoomName(PhotonNetwork.CurrentRoom.Name);
        _canvasManager.ShowCanvas(_canvasManager.CurrentRoomCanvas);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Create room failed: " + message);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        List<string> roomNames = new List<string>();
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList || !info.IsOpen || !info.IsVisible)
            {
                if (_cachedRoomInfo.Contains(info)) _cachedRoomInfo.Remove(info);
            }
            else if (!_cachedRoomInfo.Contains(info)) _cachedRoomInfo.Add(info);
        }

        foreach (RoomInfo info in _cachedRoomInfo)
        {
            roomNames.Add(info.Name);
        }
        _canvasManager.OnRoomListUpdate(roomNames);
    }
    public override void OnJoinedRoom() 
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _canvasManager.SetRoomName(PhotonNetwork.CurrentRoom.Name);
        UpdatePlayerList();
        _canvasManager.ShowCanvas(_canvasManager.CurrentRoomCanvas);
    } 
    public override void OnJoinRoomFailed(short returnCode, string message) => Debug.LogError(message);
    public override void OnPlayerEnteredRoom(Player newPlayer) => UpdatePlayerList(); 
    public override void OnPlayerLeftRoom(Player otherPlayer) => UpdatePlayerList(); 
    #endregion
}

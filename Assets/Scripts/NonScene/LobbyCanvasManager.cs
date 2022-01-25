using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyCanvasManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private LobbyManager _lobbyManager;
    [SerializeField] private List<GameObject> _canvases;


    [Header("Connection canvas")]
    [SerializeField] private GameObject _connectionCanvas;
    public GameObject ConnectionCanvas => _connectionCanvas;


    [Header("Lost connection canvas")]
    [SerializeField] private GameObject _lostConnectionCanvas;
    public GameObject LostConnectionCanvas => _lostConnectionCanvas;


    [Header("Lobby canvas")]
    [SerializeField] private GameObject _lobbyCanvas;
    [SerializeField] private TMP_InputField _nicknameInputField;
    public GameObject LobbyCanvas => _lobbyCanvas;


    [Header("Create room canvas")]
    [SerializeField] private GameObject _createRoomCanvas;
    [SerializeField] private TMP_InputField roomNameInputField;
    public GameObject CreateRoomCanvas => _createRoomCanvas;


    [Header("Current room canvas")]
    [SerializeField] private GameObject _currentRoomCanvas;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private Transform _playerListContent;
    [SerializeField] private TextMeshProUGUI _roomName;
    [SerializeField] private TextMeshProUGUI _nicknameTextPrefab;
    private readonly List<GameObject> _cachedNickNames = new List<GameObject>();
    public GameObject CurrentRoomCanvas => _currentRoomCanvas;


    [Header("Room list canvas")]
    [SerializeField] private GameObject _roomListCanvas;
    [SerializeField] private Transform _roomListContent;
    [SerializeField] private GameObject _joinRoomButtonPrefab;
    private readonly List<GameObject> _joinRoomButtons = new List<GameObject>();
    public GameObject RoomListCanvas => _roomListCanvas;


    private void Start()
    {
        ShowCanvas(_connectionCanvas);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_currentRoomCanvas.activeSelf) OnClickLeaveRoom();
            else if (RoomListCanvas.activeSelf) OnClickLeaveRoomList();
            else if (CreateRoomCanvas.activeSelf) OnClickLeaveCreateRoom();
            else OnClickExit();
        }
    }
    public void ShowCanvas(GameObject canvas)
    {
        foreach (GameObject p in _canvases)
        {
            if (p == canvas) p.SetActive(true);
            else if (p.activeSelf) p.SetActive(false);
        }
    }

    #region lostConnectionCanvas

    public void OnClickConnect()
    {
        _lobbyManager.Connect();
    }

    #endregion

    #region lobbyCanvas
    public void OnChangedNickname() => _lobbyManager.ChangeNickname(_nicknameInputField.text);
    public void OnClickCreateRoom() => ShowCanvas(CreateRoomCanvas);
    public void OnClickRoomList() => ShowCanvas(RoomListCanvas);
    public void OnClickExit() => Application.Quit();
    #endregion

    #region CreateRoomCanvas
    public void OnClickSubmit() => _lobbyManager.CreateRoom(roomNameInputField.text);
    public void OnClickLeaveCreateRoom() => ShowCanvas(_lobbyCanvas);
    #endregion

    #region CurrentRoomCanvas
    public void OnClickLeaveRoom() => _lobbyManager.LeaveRoom();
    public void OnClickStart() => _lobbyManager.LoadGame();
    public void SetRoomName(string roomName) => _roomName.text = roomName;
    public void SetRootRules(bool rootRules) => _startButton.SetActive(rootRules);
    public void OnPlayerListUpdate(List<string> nickNames)
    {
        foreach (GameObject nickName in _cachedNickNames) Destroy(nickName);
        _cachedNickNames.Clear();

        foreach (string nickName in nickNames)
        {
            GameObject nickNameText = Instantiate(_nicknameTextPrefab.gameObject, _playerListContent);
            nickNameText.GetComponent<TextMeshProUGUI>().text = nickName;

            _cachedNickNames.Add(nickNameText);
        }
    }
    #endregion

    #region RoomListCanvas
    public void OnClickLeaveRoomList() => ShowCanvas(_lobbyCanvas);
    public void OnRoomListUpdate(List<string> roomNames)
    {
        foreach (GameObject button in _joinRoomButtons) Destroy(button);
        _joinRoomButtons.Clear();

        foreach (string roomName in roomNames)
        {
            GameObject button = Instantiate(_joinRoomButtonPrefab, _roomListContent);
            button.GetComponent<JoinRoomButton>().Text = roomName;
            _joinRoomButtons.Add(button);
        }
    }
    public void OnClickJoinRoom(string roomName) => _lobbyManager.JoinRoom(roomName);
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JoinRoomButton : MonoBehaviour
{
    public TextMeshProUGUI _buttonText;
    public string Text
    {
        get { return _buttonText.text; }
        set { _buttonText.text = value; }
    }
    private MainMenuGUI _lobbyCanvasManager;
    private void Start()
    {
        _lobbyCanvasManager = FindObjectOfType<MainMenuGUI>();
    }
    public void OnClickJoinRoom()
    {
        _lobbyCanvasManager.OnClickJoinRoom(Text);
    }



}

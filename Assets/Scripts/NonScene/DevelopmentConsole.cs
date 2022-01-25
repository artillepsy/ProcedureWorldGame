using UnityEngine;
using TMPro;
using System.Collections;

public class DevelopmentConsole : MonoBehaviour
{
    [SerializeField] private  KeyCode _showHideCode = KeyCode.Slash;
    [SerializeField] private KeyCode _applyCommandCode = KeyCode.Return;
    [SerializeField] private TMP_InputField _inputField;

    [Header("Message box")]
    [SerializeField] TextMeshProUGUI _messageBox;
    [SerializeField] private float _disappearTimeSec = 3f;
    [SerializeField] private float _visibleTimeSec = 2f;

    private float _alpha;
    private Vector3 _oldMsgBoxPos;
    private Vector3 _newMsgBoxPos;
    private Coroutine coroutine;
    private void Awake()
    {
        _alpha = _messageBox.alpha;
        _oldMsgBoxPos = _messageBox.transform.position;
        _newMsgBoxPos = new Vector3(_oldMsgBoxPos.x, _inputField.transform.position.y - 2f, _oldMsgBoxPos.y);
        _messageBox.gameObject.SetActive(false);
        CommandManager.OnAnswerReady.AddListener(ShowMessage);
        ShowMode(false);
    }
    private void ShowMessage(string message)
    {
        _messageBox.alpha = _alpha;
        _messageBox.text = message;
        _messageBox.gameObject.SetActive(true);

        if(coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(VisibilityChangeRoutine());
    }
    private IEnumerator VisibilityChangeRoutine()
    {
        _messageBox.alpha = _alpha;
        yield return new WaitForSeconds(_visibleTimeSec);
        
        float alphaStep = Time.deltaTime / _disappearTimeSec;

        while(_messageBox.alpha>=0)
        {
            _messageBox.alpha -= alphaStep;
            yield return null;
        }
        _messageBox.gameObject.SetActive(false);
        coroutine = null;
    }
    private void ShowMode(bool mode)
    {
        if(mode == true) _messageBox.transform.position = _oldMsgBoxPos;
        else _messageBox.transform.position = _newMsgBoxPos;

        _inputField.gameObject.SetActive(mode);
        CustomEventManager.ChangeInputPermission(!mode);
    }
    void Update()
    {
        InputKey();
    }
    private void InputKey()
    {
        if(Input.GetKeyDown(_showHideCode))
        {
            ShowMode(!_inputField.gameObject.activeSelf);
            _inputField.text = "";
            _inputField.ActivateInputField();
        }
        if(Input.GetKeyDown(_applyCommandCode))
        {
            OnEndEdit();
        }
    }
    public void OnEndEdit()
    {
        if (!_inputField.gameObject.activeSelf) return;
        if (_inputField.text != "") CommandManager.RecieveCommand(_inputField.text);
        _inputField.text = "";
        ShowMode(false);
    }
}

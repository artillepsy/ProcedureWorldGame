using UnityEngine;
using TMPro;
using System.Collections;

public class CheatConsole : MonoBehaviour
{
    [SerializeField] private KeyCode showHideCode = KeyCode.Slash;
    [SerializeField] private KeyCode applyCommandCode = KeyCode.Return;
    [SerializeField] private TMP_InputField inputField;

    [Header("Message box")]
    [SerializeField] TextMeshProUGUI messageBox;
    [SerializeField] private float disappearTimeSec = 5f;
    [SerializeField] private float visibleTimeSec = 2f;

    private float _alpha;
    private Vector3 _oldMsgBoxPos;
    private Vector3 _newMsgBoxPos;
    private Coroutine coroutine;
    private void Awake()
    {
        _alpha = messageBox.alpha;
        _oldMsgBoxPos = messageBox.transform.position;
        _newMsgBoxPos = new Vector3(_oldMsgBoxPos.x, inputField.transform.position.y - 2f, _oldMsgBoxPos.y);
        messageBox.gameObject.SetActive(false);
        CommandHandler.OnAnswerReady.AddListener(ShowMessage);
        SetShowMode(false);
    }
    private void ShowMessage(string message)
    {
        messageBox.alpha = _alpha;
        messageBox.text = message;
        messageBox.gameObject.SetActive(true);

        if(coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(VisibilityChangeCoroutine());
    }
    private IEnumerator VisibilityChangeCoroutine()
    {
        messageBox.alpha = _alpha;
        yield return new WaitForSeconds(visibleTimeSec);
        
        float alphaStep = Time.deltaTime / disappearTimeSec;

        while(messageBox.alpha>=0)
        {
            messageBox.alpha -= alphaStep;
            yield return null;
        }
        messageBox.gameObject.SetActive(false);
        coroutine = null;
    }
    private void SetShowMode(bool mode)
    {
        if(mode == true) messageBox.transform.position = _oldMsgBoxPos;
        else messageBox.transform.position = _newMsgBoxPos;

        inputField.gameObject.SetActive(mode);
        CustomEventHandler.ChangeInputPermission(!mode);
    }
    void Update()
    {
        InputKey();
    }
    private void InputKey()
    {
        if(Input.GetKeyDown(showHideCode))
        {
            SetShowMode(!inputField.gameObject.activeSelf);
            inputField.text = "";
            inputField.ActivateInputField();
        }
        if(Input.GetKeyDown(applyCommandCode))
        {
            OnEndEdit();
        }
    }
    public void OnEndEdit()
    {
        if (!inputField.gameObject.activeSelf) return;
        if (inputField.text != "") CommandHandler.Recieve(inputField.text);
        inputField.text = "";
        SetShowMode(false);
    }
}

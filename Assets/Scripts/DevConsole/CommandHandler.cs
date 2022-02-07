using Core;
using UnityEngine.Events;

public static class CommandHandler
{
    private static bool _enabled = false;

    public static UnityEvent<string> OnAnswerReady = new UnityEvent<string>();
    public static UnityEvent<float> OnPlayerSpeedChanged = new UnityEvent<float>();
    public static UnityEvent<bool> OnColliderVisibilityChanged = new UnityEvent<bool>();
    private static void ChangeCheatMode()
    {
        _enabled = !_enabled;
        if (_enabled) OnAnswerReady?.Invoke("Cheat console ON");
        else OnAnswerReady?.Invoke("Cheat console OFF");
    }
    private static bool CheckFormat(string command)
    {
        command = command.ToLower();

        string[] values = command.Split(' ');

        if(values.Length < 2)
        {
            OnAnswerReady?.Invoke("Error. Unknown command");
            return false;
        }
        return true;
    }
    public static void Recieve(string command)
    {
        if (command.Equals(Constants.Commands.Password))
        {
            ChangeCheatMode();
            return;
        }
        else if (!_enabled)
        {
            OnAnswerReady?.Invoke("Error. Cheat console isn't activated");
            return;
        }
        else if (!CheckFormat(command)) return;

        command = command.ToLower();
        var values = command.Split(' ');
        Handle(values);
    }
    private static void Handle(string[] values)
    {
        switch (values[0])
        {
            case Constants.Commands.PlayerSpeed:
                ChangePlayerSpeed(values[1]);
                break;
            case Constants.Commands.ColliderEnableMode:
                ChangeColliderEnableMode(values[1]);
                break;
            default:
                OnAnswerReady?.Invoke("Error. Unknown command");
                break;
        }
    }
    private static void ChangePlayerSpeed(string strSpeed)
    {
        float speed;
        
        if(float.TryParse(strSpeed, out speed) == false || speed <= 0)
        {
            OnAnswerReady?.Invoke("Invalid speed value");
            return;
        }

        OnAnswerReady?.Invoke("Player speed changed");
        OnPlayerSpeedChanged?.Invoke(speed);
    }
    private static void ChangeColliderEnableMode(string strMode)
    {
        bool mode;

        if (bool.TryParse(strMode, out mode) == false)
        {
            OnAnswerReady?.Invoke("Invalid collider enable mode");
            return;
        }

        OnAnswerReady?.Invoke("Collider enabled: "+ mode);
        OnColliderVisibilityChanged?.Invoke(mode);
    }
}

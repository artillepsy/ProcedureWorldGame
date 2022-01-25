using UnityEngine.Events;

public static class CommandManager
{
    private static bool _enabled = false;
    private const string _password = "((+))";

    public static UnityEvent<float> OnPlayerSpeedChanged = new UnityEvent<float>();
    public static UnityEvent<string> OnAnswerReady = new UnityEvent<string>();
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
    public static void RecieveCommand(string command)
    {
        if (command.Equals(_password))
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
        string[] values = command.Split(' ');
        switch (values[0])
        {
            case Constants.Commands.PlayerSpeed:
                ChangePlayerSpeed(values[1]);
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
}

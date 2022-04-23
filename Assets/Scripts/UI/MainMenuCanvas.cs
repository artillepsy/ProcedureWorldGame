using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuCanvas : MonoBehaviour
    {
        public void OnClickStart() => SceneManager.LoadScene("Game");
        public void OnClickExit() => Application.Quit();
    }
}
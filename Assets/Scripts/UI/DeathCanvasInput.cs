using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class DeathCanvasInput : MonoBehaviour
    {
        public void OnClickRestart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync("Game");
        }

        public void OnClickMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
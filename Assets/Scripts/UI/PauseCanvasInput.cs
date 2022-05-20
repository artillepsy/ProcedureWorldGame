using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseCanvasInput : MonoBehaviour
    {
        [SerializeField] private GameObject pauseCanvas;
        [SerializeField] private List<GameObject> canvasesToHide;

        private void Awake() => pauseCanvas.SetActive(false);

        public void OnClickPause()
        {
            Time.timeScale = 0f;
            SetPauseStatus(true);
        }

        public void OnClickResume()
        {
            Time.timeScale = 1;
            SetPauseStatus(false);
        }

        public void OnClickMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync("MainMenu");
        }

        private void SetPauseStatus(bool status)
        {
            pauseCanvas.SetActive(status);

            foreach (var canvas in canvasesToHide)
            {
                canvas.SetActive(!status);
            }
        }
    }
}
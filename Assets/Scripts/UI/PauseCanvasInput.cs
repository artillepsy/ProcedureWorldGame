using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseCanvasInput : MonoBehaviour
    {
        [SerializeField] private GameObject pauseCanvas;
        [SerializeField] private List<GameObject> canvasesToHide;
        public static UnityEvent<bool> OnGamePaused = new UnityEvent<bool>();

        private void Awake() => pauseCanvas.SetActive(false);

        public void OnClickPause()
        {
            SetPauseStatus(true);
            OnGamePaused?.Invoke(true);
        }

        public void OnClickResume()
        {
            SetPauseStatus(false);
            OnGamePaused?.Invoke(false);
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
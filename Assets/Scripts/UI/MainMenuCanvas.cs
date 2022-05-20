using Experience;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject craftCanvas;
        [SerializeField] private GameObject mainCanvas;
        [SerializeField] private Animator animator;
        private static readonly int ToCraft = Animator.StringToHash("ToCraft");
        private static readonly int ToMain = Animator.StringToHash("ToMain");

        private void Awake()
        {
            craftCanvas.SetActive(false);
            mainCanvas.SetActive(true);
            
            var userData = SaveSystem.Load();
            if (userData == null)
            {
                Debug.Log(userData);
                userData = new UserData(0, 0,0);
                SaveSystem.Save(userData);
            }
        }

        private void Update()
        {
            if (!UnityEngine.Input.GetKeyDown(KeyCode.C)) return;
            var userData = new UserData(1,1,0);
        }

        public void OnClickCraft() => ShowCraftCanvas(true);
        public void OnClickBack() => ShowCraftCanvas(false);
        
        public void OnClickStart() => SceneManager.LoadScene("Game");
        public void OnClickExit() => Application.Quit();

        private void ShowCraftCanvas(bool status)
        {
            animator.SetTrigger(status ? ToCraft : ToMain);
            craftCanvas.SetActive(status);
            mainCanvas.SetActive(!status);
        }
    }
}
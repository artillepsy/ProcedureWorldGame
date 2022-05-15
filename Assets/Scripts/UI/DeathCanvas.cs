using System.Collections.Generic;
using Enemy;
using Enemy.Management;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DeathCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject deadCanvas;
        [SerializeField] private TextMeshProUGUI killsLabel;
        [SerializeField] private TextMeshProUGUI timeLabel;
        [SerializeField] private TextMeshProUGUI wavesLabel;
        [SerializeField] private List<GameObject> canvasesToHide;
        private int _kills = 0;
        private int _wave = 0;
        private int _time = 0;

        private void Awake() => deadCanvas.SetActive(false);

        private void Start()
        {
            PlayerHealth.OnDied.AddListener(ShowDeadCanvas);
            EnemyHealth.OnEnemyDie.AddListener(()=> _kills++);
            EnemySpawner.OnStartWave.AddListener((wave) => _wave = wave);
            InvokeRepeating(nameof(IncrementSeconds), 1, 1);
        }

        private void ShowDeadCanvas()
        {
            killsLabel.text = _kills.ToString();
            wavesLabel.text = _wave.ToString();
            timeLabel.text = TimeToString();
            
            foreach (var canvas in canvasesToHide)
            {
                canvas.SetActive(false);
            }
            deadCanvas.SetActive(true);

            Time.timeScale = 0f;
            CancelInvoke(nameof(IncrementSeconds));
        }

        private void IncrementSeconds()
        {
            if (Time.timeScale == 0f) return;
            _time++;
        }

        private string TimeToString()
        {
            var s = "";
            var hours = _time / 3600;
            if (hours > 0)
            {
                s = hours + ":";
                _time %= 3600;
            }
            var minutes = _time / 60;
            if (minutes > 0)
            {
                s += minutes + ":";
                _time %= 60;
            }
            s += _time.ToString();
            return s;
        }
    }
}
using System.Collections.Generic;
using Enemy;
using Enemy.Management;
using Experience;
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
        [SerializeField] private TextMeshProUGUI experienceLabel;
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

        private void Update()
        {
            if (!UnityEngine.Input.GetKeyDown(KeyCode.K)) return;
            PlayerHealth.OnDied.Invoke();
        }

        private void ShowDeadCanvas()
        {
            killsLabel.text = _kills.ToString();
            wavesLabel.text = _wave.ToString();
            timeLabel.text = TimeToString();
            
            var exp = ExperienceCalculator.Inst.CalculateExperience(_kills, _wave, _time);
            experienceLabel.text = exp.ToString();
            
            SaveProgress(exp);
            
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

        private void SaveProgress(int exp)
        {
            var userData = SaveSystem.Load();
            userData.GrenadeCount = FindObjectOfType<GrenadeThrower>().Count;
            userData.FreezersCount = FindObjectOfType<TimeFreezer>().Count;
            userData.ExpCount += exp;
            SaveSystem.Save(userData);
        }

        private string TimeToString()
        {
            var s = "";
            var hours = _time / 3600;
            s = hours + ":";
            if (hours > 0) _time %= 3600;
            var minutes = _time / 60;
            s += minutes + ":";
            if (minutes > 0) _time %= 60;
            s += _time;
            return s;
        }
    }
}
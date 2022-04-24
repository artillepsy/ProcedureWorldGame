using Enemy.Management;
using TMPro;
using UnityEngine;

namespace UI
{
    public class WaveLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI waveLabel;
        private Animation _anim;

        private void OnEnable()
        {
            _anim = GetComponent<Animation>();
            EnemySpawner.OnStartWave.AddListener(PlayWaveAnim);
        }

        private void PlayWaveAnim(int number)
        {
            waveLabel.text = "Wave " + number;
            _anim.Play();
        }
    }
}
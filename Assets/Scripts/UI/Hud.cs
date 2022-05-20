using Player;
using TMPro;
using UnityEngine;
using Weapons;
using Slider = UnityEngine.UI.Slider;

namespace UI
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammoInClipText;
        [SerializeField] private TextMeshProUGUI grenadeCountText;
        [SerializeField] private TextMeshProUGUI freezersCountText;
        [SerializeField] private TextMeshProUGUI totalAmmoCountText;
        [SerializeField] private Slider healthBar;

        private int _totalAmmoCount;
        private int _clipSize;
        private int _ammoInClip;
        private void OnEnable()
        {
            Weapon.OnAmmoCountChange.AddListener(UpdateAllAmmoData);
            Weapon.OnShoot.AddListener(DecrementClipSize);
            FindObjectOfType<PlayerHealth>().OnHealthChanged.AddListener(UpdateHealthStats);
            FindObjectOfType<GrenadeThrower>().OnGrenadeCountChange.AddListener(UpdateGrenadeCount);
            FindObjectOfType<TimeFreezer>().OnFreezersCountChange.AddListener(UpdateFreezersCount);
        }

        private void Start()
        {
            healthBar.value = 1f;
        }

        private void UpdateGrenadeCount(int count) => grenadeCountText.text = count.ToString();
        private void UpdateFreezersCount(int count) => freezersCountText.text = count.ToString();

        private void DecrementClipSize(int ammoInClip)
        {
            _ammoInClip = ammoInClip;
            ammoInClipText.text = _ammoInClip + " / " + _clipSize;
            totalAmmoCountText.text = _totalAmmoCount.ToString();
        }
        
        private void UpdateAllAmmoData(int totalAmmoCount, int clipSize, int ammoInClip)
        {
            _totalAmmoCount = totalAmmoCount;
            _clipSize = clipSize;
            _ammoInClip = ammoInClip;
            
            ammoInClipText.text = _ammoInClip + " / " + _clipSize;
            totalAmmoCountText.text = _totalAmmoCount.ToString();
        }
        private void UpdateHealthStats(float totalHealth, float maxHealth)
        {
            healthBar.value = totalHealth / maxHealth;
        }
    }
}
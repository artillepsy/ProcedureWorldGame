using Experience;
using TMPro;
using UnityEngine;

namespace UI.Craft
{
    public class CraftCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI grenadesLabel;
        [SerializeField] private TextMeshProUGUI freezersLabel;
        [SerializeField] private TextMeshProUGUI expLabel;
        [SerializeField] private TextMeshProUGUI grenadesCostLabel;
        [SerializeField] private TextMeshProUGUI freezersCostLabel;
        [SerializeField] private int grenadesCreateCost = 1000;
        [SerializeField] private int freezersCreateCost = 1000;
        
        private int _grenades;
        private int _freezers;
        private int _exp;

        public void OnClickCreateGrenades()
        {
            if (grenadesCreateCost > _exp) return;
            _exp -= grenadesCreateCost;
            _grenades++;
            UpdateInfo();
        } 
        public void OnClickCreateFreezers()
        {
            if (freezersCreateCost > _exp) return;
            _exp -= freezersCreateCost;
            _freezers++;
            UpdateInfo();
        }

        private void Awake()
        {
            grenadesCostLabel.text = grenadesCreateCost + " exp";
            freezersCostLabel.text = freezersCreateCost + " exp";
        }

        private void OnEnable()
        {
            var userData = SaveSystem.Load();
            _grenades = userData.GrenadeCount;
            _freezers = userData.FreezersCount;
            _exp = userData.ExpCount;
            UpdateInfo();
        }

        private void OnDisable()
        {
            var userData = new UserData(_grenades, _freezers, _exp);
            SaveSystem.Save(userData);
        }

        private void UpdateInfo()
        {
            grenadesLabel.text = _grenades.ToString();
            freezersLabel.text = _freezers.ToString();
            expLabel.text = _exp.ToString();
        }
    }
}
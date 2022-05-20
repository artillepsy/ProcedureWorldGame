using Experience;
using TMPro;
using UnityEngine;

namespace UI.Craft
{
    public class CraftCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI grenadesLabel;
        [SerializeField] private TextMeshProUGUI expLabel;
        [SerializeField] private TextMeshProUGUI costLabel;
        [SerializeField] private int createCost = 1000;
        
        private int _grenades;
        private int _exp;

        public void OnClickCreateGrenades()
        {
            if (createCost > _exp) return;
            _exp -= createCost;
            _grenades++;
            UpdateInfo();
        }

        private void Awake() => costLabel.text = "Craft ("+ createCost + ")";
        
        private void OnEnable()
        {
            var userData = SaveSystem.Load();
            _grenades = userData.GrenadeCount;
            _exp = userData.ExpCount;
            UpdateInfo( );
        }

        private void OnDisable()
        {
            var userData = new UserData(_grenades, _exp);
            SaveSystem.Save(userData);
        }

        private void UpdateInfo()
        {
            grenadesLabel.text = _grenades.ToString();
            expLabel.text = _exp.ToString();
        }
    }
}
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class EnemyNameRandomizer : MonoBehaviour
    {
        [SerializeField] private List<string> enemyNames;

        private void Awake() => GetComponent<TextMeshProUGUI>().text = enemyNames[Random.Range(0, enemyNames.Count)];
    }
}
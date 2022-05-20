using UnityEngine;

namespace Experience
{
    public class ExperienceCalculator : MonoBehaviour
    {
        [SerializeField] private float killsKoef = 0.4f;
        [SerializeField] private float waveKoef = 0.1f;
        [SerializeField] private float timeKoef = 0.1f;
        
        public static ExperienceCalculator Inst { get; private set; }
                
        public int CalculateExperience(int kills, int wave, float time)
        {
            var exp = kills * killsKoef * wave * waveKoef * time * timeKoef;
            return Mathf.RoundToInt(exp);
        }

        private void Awake() => Inst = this;
    }
}
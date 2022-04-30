using Player;
using UnityEngine;

namespace UI
{
    public class PlayerDamaged : MonoBehaviour
    {
        private Animation _anim;
        private void OnEnable()
        {
            _anim = GetComponent<Animation>();
            PlayerHealth.OnDamaged.AddListener(() => _anim.Play());
        }
    }
}
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 2f;
        [SerializeField] private float minStartSpeed = 3f;
        [SerializeField] private float maxStartSpeed = 5.5f;
        [SerializeField] private float deviation = 10f;
        
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private AnimationCurve speedCurve;
        [SerializeField] private Rigidbody rb;
        private Vector3 _direction;
        private float _time = 0f;
        private float _speed;
        private void Awake()
        {
            _speed = Random.Range(minStartSpeed, maxStartSpeed);            
            Destroy(gameObject, lifeTime);
        }

        public void SetValues(float damage, Vector3 direction)
        {
            label.text = Mathf.RoundToInt(damage).ToString();
            var rotation = Quaternion.Euler(0, Random.Range(-deviation, deviation), 0);
            _direction = rotation * direction;
        }

        private void FixedUpdate()
        {
            _time += Time.fixedDeltaTime;
            var speed = speedCurve.Evaluate(_time/lifeTime) * _speed;
            rb.velocity = _direction * speed;
        }
    }
}
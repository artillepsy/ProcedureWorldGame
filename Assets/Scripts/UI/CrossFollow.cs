using UnityEngine;

namespace UI
{
    public class CrossFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private Camera _cam;
        private RectTransform _transform;

        private void Start()
        {
            _cam = Camera.main;
            _transform = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            //_cam.ResetWorldToCameraMatrix();
            var pos = _cam.WorldToScreenPoint(target.position);
            _transform.SetPositionAndRotation(pos, Quaternion.identity); 
        }
    }
}
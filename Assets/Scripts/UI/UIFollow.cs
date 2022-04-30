using UnityEngine;

namespace UI
{
    public class UIFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private RectTransform _transform;
        public RectTransform RectTrans => _transform;
        public Transform Target => target;
        private void Start()
        {
            _transform = GetComponent<RectTransform>();
            InfoCanvas.Inst.AddUI(this);
        }
    }
}
using UnityEngine;

namespace UI
{
    public class UIFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private RectTransform _transform;
        private void Start()
        {
            _transform = GetComponent<RectTransform>();
            InfoCanvas.Inst.AddUI(_transform, target);
        }
    }
}
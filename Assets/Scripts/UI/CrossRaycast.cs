using UnityEngine;

namespace UI
{
    public class CrossRaycast : MonoBehaviour
    {
        [SerializeField] private Transform crossPoint;
        [SerializeField] private float distance = 5f;
        private void Update()
        {
            var ray = new Ray(transform.position, transform.forward);
            var hits = Physics.RaycastAll(ray, distance);
            var dist = distance;
            foreach (var hit in hits)
            {
                if (hit.collider.isTrigger) continue;
                dist = hit.distance;
                break;
            }

            var point = transform.position + transform.forward * dist;
            crossPoint.position = point;
        }
    }
}
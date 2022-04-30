using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace UI
{
    public class InfoCanvas : MonoBehaviour
    {
        private Dictionary<RectTransform, Transform> _pairs;
        private RectTransform _transform;
        private Camera _cam;

        public static InfoCanvas Inst;

        public void AddUI(RectTransform ui, Transform target)
        {
            _pairs.Add(ui, target);
            ui.SetParent(_transform);
        }

        private void Awake()
        {
            Inst = this;
            _cam = Camera.main;
            _pairs = new Dictionary<RectTransform, Transform>();
            _transform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            CinemachineCore.CameraUpdatedEvent.AddListener((brain) => UpdatePos());
        }

        private void Update()
        {
            foreach (var pair in _pairs)
            {
                if (pair.Value) continue;
                pair.Key.SetParent(null);
                _pairs.Remove(pair.Key);
                Destroy(pair.Key.gameObject);
            }
        }

        private void UpdatePos()
        {
            foreach (var pair in _pairs)
            {
                var pos = _cam.WorldToScreenPoint(pair.Value.position);
                pair.Key.SetPositionAndRotation(pos, Quaternion.identity); 
            }
        }
    }
}
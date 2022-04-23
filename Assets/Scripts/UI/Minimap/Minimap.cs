using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace UI.Minimap
{
    public class Minimap : MonoBehaviour
    {
        [SerializeField] private Camera minimapCamera;
        [SerializeField] private List<IconPrefab> iconPrefabList;
        public static Minimap Inst = null;
        private List<Icon> _icons;
        private float _clampValue;

        public void AddIcon(Icon icon)
        {
            foreach (IconPrefab iconPrefab in iconPrefabList)
            {
                if (iconPrefab.Type != icon.Type) continue;

                icon.Instance = Instantiate(iconPrefab.Prefab, icon.transform);
                icon.Instance.transform.position = new Vector3(
                    icon.transform.position.x,
                    iconPrefab.Prefab.transform.localPosition.y,
                    icon.transform.position.z);
                if (iconPrefab.ShouldClamp) _icons.Add(icon);
                break;
            }
        }

        public void RemoveIcon(Icon icon)
        {
            if (_icons.Contains(icon)) _icons.Remove(icon);
        }

        private void Awake()
        {
            _icons = new List<Icon>();
            _clampValue = minimapCamera.orthographicSize;
            if (Inst == null) Inst = this;
        }

        private void Update()
        {
            var cameraPosition = minimapCamera.transform.position;
            var clampMinX = cameraPosition.x - _clampValue;
            var clampMaxX = cameraPosition.x + _clampValue;
            var clampMinZ = cameraPosition.z - _clampValue;

            var clampMaxZ = cameraPosition.z + _clampValue;
            foreach (Icon icon in _icons)
            {
                if (!icon.gameObject.activeSelf)
                {
                    continue;
                }

                var iconPosition = icon.transform.position;
                var clampX = Mathf.Clamp(iconPosition.x, clampMinX, clampMaxX);
                var clampZ = Mathf.Clamp(iconPosition.z, clampMinZ, clampMaxZ);
                icon.Instance.transform.position = new Vector3(clampX, icon.Instance.transform.position.y, clampZ);
            }
        }

        [System.Serializable]
        private class IconPrefab
        {
            [SerializeField] private GameObject prefab;
            [SerializeField] private float scale;
            [SerializeField] private bool shouldClamp;
            [SerializeField] private Constants.Icons.Type type;

            public GameObject Prefab => prefab;
            public float Scale => scale;
            public bool ShouldClamp => shouldClamp;
            public Constants.Icons.Type Type => type;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class InfoCanvas : MonoBehaviour
    {
        private List<UIFollow> _elems = new List<UIFollow>();
        private RectTransform _transform;
        private Camera _cam;

        public static InfoCanvas Inst;

        public void AddUI(UIFollow uiElem)
        {
            _elems.Add(uiElem);
            uiElem.RectTrans.SetParent(_transform);
        }

        private void Awake()
        {
            Inst = this;
            _cam = Camera.main;
            _transform = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            var elemsToRemove = new List<UIFollow>();
            foreach (var elem in _elems)
            {
                if (!elem.Target)
                {
                    elem.RectTrans.SetParent(null);
                    elemsToRemove.Add(elem);
                }
                else
                {
                    var pos = _cam.WorldToScreenPoint(elem.Target.position);
                    elem.RectTrans.SetPositionAndRotation(pos, Quaternion.identity); 
                }
            }
            foreach (var elem in elemsToRemove)
            {
                _elems.Remove(elem);
                Destroy(elem.gameObject);
            }
        }
    }
}
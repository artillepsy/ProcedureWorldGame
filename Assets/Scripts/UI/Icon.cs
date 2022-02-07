using System;
using Core;
using UnityEngine;

namespace UI.Minimap
{ 
    [DisallowMultipleComponent]
    public class Icon : MonoBehaviour
    {
        [SerializeField] private Constants.Icons.Type type;
        public GameObject Instance { set; get; }
        public bool ShouldClamp { set; get; }
        public Constants.Icons.Type Type => type;
        private void Start()
        {
            Minimap.Inst.AddIcon(this);
        }

        private void OnDestroy()
        {
            Minimap.Inst.RemoveIcon(this);
        }
    }
}

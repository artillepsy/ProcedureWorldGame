using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class VisibilityChange : MonoBehaviour
    {
        private List<MeshRenderer> _renderers;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<MeshRenderer>().ToList();
        }

        private void OnBecameVisible()
        {
            if (_renderers[0].enabled) return;
            foreach (var meshRenderer in _renderers)
            {
                if(meshRenderer.enabled) continue;
                meshRenderer.enabled = true;
            }
        }

        private void OnBecameInvisible()
        {
            if (!_renderers[0].enabled) return;
            foreach (var meshRenderer in _renderers)
            {
                if(!meshRenderer.enabled) continue;
                meshRenderer.enabled = false;
            }
        }
    }
}
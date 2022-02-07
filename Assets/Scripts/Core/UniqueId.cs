using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class UniqueId : MonoBehaviour
    {
        private static int _nextInstId = 0;
        public int Id { get; set; }
        public int InstId { get; private set; }
        public void GetPrefabId()
        {
            Debug.Log(Id);
        }
        private void Awake()
        {
            InstId = _nextInstId;
            _nextInstId++;
        }
    }
}

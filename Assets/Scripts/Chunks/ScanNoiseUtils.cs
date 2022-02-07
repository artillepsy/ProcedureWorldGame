using System.Collections;
using System.Collections.Generic;
using Core;
using Player;
using UnityEngine;
namespace Chunks
{
    public class ScanNoiseUtils : MonoBehaviour
    {
        [Header("Noise settings")]
        [SerializeField] private float nodeSize = 300;
        [Min(3)]
        [SerializeField] private int inDimension = 3;
        [Min(5)]
        [SerializeField] private int outDimension = 5;
        [Header("Debug settings")]
        [SerializeField] private bool showNodePoins = true;
        [SerializeField] private float nodePointSize = 3f;
        private float _halfNodeSize;
        private int _minInRadius;
        private int _maxInRadius;
        protected Transform _player;
        private Dictionary<Vector3, CachedPointInfo> _cachedPointsDictionary;
        private List<CachedPointInfo> _inDimensionPointsList;
        protected void Awake()
        {
            _player = FindObjectOfType<Movement>().transform;
            _halfNodeSize = nodeSize / 2;
            _cachedPointsDictionary = new Dictionary<Vector3, CachedPointInfo>();
            _inDimensionPointsList = new List<CachedPointInfo>();
            _minInRadius = (outDimension - inDimension) / 2;
            _maxInRadius = (outDimension + inDimension) / 2;
        }
        protected void AddPointToNode(Vector3 position ,List<PlacementPrefabInfo> prefabInfoList) // before scan
        {
            _inDimensionPointsList.Clear();
            int i, j;
            float x, z;
            var start = VectorHelper.GetConvertedStartPoint(position, outDimension, nodeSize);
            for (x = start.x, i = 0; i < outDimension; i++, x = start.x + nodeSize * i)
            {
                for (z = start.z, j = 0; j < outDimension; j++, z = start.z + nodeSize * j)
                {
                    if (!VectorHelper.IsInEnableArea(i, j, _minInRadius, _maxInRadius)) continue;
                    var checkPoint = new Vector3(x, 0, z);
                    if (_cachedPointsDictionary.TryGetValue(checkPoint, out var buffer))
                    {
                        _inDimensionPointsList.Add(buffer);
                        continue;
                    }
                    var left = checkPoint.x - _halfNodeSize;
                    var right = checkPoint.x + _halfNodeSize;
                    var up = checkPoint.z + _halfNodeSize;
                    var down = checkPoint.z - _halfNodeSize;
                    var randomPointInCurrentNode = VectorHelper.GetRandomPointInBounds(left, right, down, up);
                    var id = ObjectPool.GetIdByProbability(prefabInfoList);
                    var cachedPoint = new CachedPointInfo(id, randomPointInCurrentNode);
                    _cachedPointsDictionary.Add(checkPoint, cachedPoint);
                    _inDimensionPointsList.Add(cachedPoint);
                }
            }
        }
        protected int GetNearestNoisePointId(Vector3 position) // return id to spawn using in scanner
        {
            var minDist = outDimension * nodeSize * 3; // custom 3 - max possible dist
            var idToReturn = -1;
            foreach (CachedPointInfo info in _inDimensionPointsList)
            {
                float distance = Vector3.Distance(position, info.Position);
                if (distance< minDist)
                {
                    idToReturn = info.Id;
                    minDist = distance;
                }
            }
            return idToReturn;
        }
       
        private void OnDrawGizmos()
        {
            if(!showNodePoins ||_inDimensionPointsList == null || _inDimensionPointsList.Count == 0)
            {
                return;
            }
            Gizmos.color = Color.cyan;
            foreach(CachedPointInfo info in _inDimensionPointsList)
            {
                Gizmos.DrawSphere(info.Position, nodePointSize);
            }
        }
        protected static class VectorHelper
        {
            public static bool IsInEnableArea(int i, int j, int minOutRadius, int maxOutRadius)
            {
                return (i >= minOutRadius && i < maxOutRadius && j >= minOutRadius && j < maxOutRadius);
            }
            public static Vector3 GetConvertedStartPoint(Vector3 pos, int outDimension, float nodeSize)
            {
                var offsetSteps = (outDimension - 1) / 2;
                var centerX = Mathf.RoundToInt((pos.x - offsetSteps * nodeSize) / nodeSize) * nodeSize;
                var centerZ = Mathf.RoundToInt((pos.z - offsetSteps * nodeSize) / nodeSize) * nodeSize;
                return new Vector3(centerX, 0, centerZ);
            }
            public static Vector3 GetRandomPointInBounds(float left, float right, float buttom, float up)
            {
                var x = Random.Range(left, right);
                var z = Random.Range(buttom, up);
                return new Vector3(x, 0, z);
            }
        }
        private class CachedPointInfo
        {
            public readonly int Id;
            public readonly Vector3 Position;
            public CachedPointInfo(int id, Vector3 position)
            {
                Id = id;
                Position = position;
            }
        }
    }
}

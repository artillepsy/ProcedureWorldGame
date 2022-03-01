using System.Collections.Generic;
using Core;
using UnityEngine;
namespace Chunks
{
    public class NoiseUtils : MonoBehaviour
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
        
        private Dictionary<Vector3, CachedPointInfo> _cachedPoints;
        private List<CachedPointInfo> _inDimensionPointsList;

        public void AddPointToNode(Vector3 position ,List<ObstaclePrefabInfo> prefabInfoList) // before scan
        {
            _inDimensionPointsList.Clear();
            int i, j;
            float x, z;
            var start = NoiseUtilsHelper.GetConvertedStartPoint(position, outDimension, nodeSize);
            for (x = start.x, i = 0; i < outDimension; i++, x = start.x + nodeSize * i)
            {
                for (z = start.z, j = 0; j < outDimension; j++, z = start.z + nodeSize * j)
                {
                    if (!NoiseUtilsHelper.IsInEnableArea(i, j, _minInRadius, _maxInRadius)) continue;
                    var checkPoint = new Vector3(x, 0, z);
                    if (_cachedPoints.TryGetValue(checkPoint, out var buffer))
                    {
                        _inDimensionPointsList.Add(buffer);
                        continue;
                    }
                    var randomPointInCurrentNode = NoiseUtilsHelper.GetRandomPointInBounds(checkPoint, _halfNodeSize);
                    var id = GameObjectPool.GetInfoByProbability(prefabInfoList).Prefab.Id;
                    var cachedPoint = new CachedPointInfo(id, randomPointInCurrentNode);
                    _cachedPoints.Add(checkPoint, cachedPoint);
                    _inDimensionPointsList.Add(cachedPoint);
                }
            }
        }
        public int GetNearestNoisePointId(Vector3 position) // return id to spawn using in scanner
        {
            var minDist = outDimension * nodeSize * 3; // custom 3 - max possible dist
            var idToReturn = -1;
            foreach (CachedPointInfo info in _inDimensionPointsList)
            {
                var distance = Vector3.Distance(position, info.Position);
                if (distance >= minDist) continue;
             
                idToReturn = info.Id;
                minDist = distance;
            }
            return idToReturn;
        }

        private void Awake()
        {
            _halfNodeSize = nodeSize / 2;
            _cachedPoints = new Dictionary<Vector3, CachedPointInfo>();
            _inDimensionPointsList = new List<CachedPointInfo>();
            _minInRadius = (outDimension - inDimension) / 2;
            _maxInRadius = (outDimension + inDimension) / 2;
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
    }
}

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
        
        private Dictionary<Vector3, CachedPointInfo> _cachedPointsDictionary;
        private List<CachedPointInfo> _inDimensionPointsList;
        private void Awake()
        {
            
            _halfNodeSize = nodeSize / 2;
            _cachedPointsDictionary = new Dictionary<Vector3, CachedPointInfo>();
            _inDimensionPointsList = new List<CachedPointInfo>();
            _minInRadius = (outDimension - inDimension) / 2;
            _maxInRadius = (outDimension + inDimension) / 2;
        }
        public void AddPointToNode(Vector3 position ,List<PlacementPrefabInfo> prefabInfoList) // before scan
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
                    if (_cachedPointsDictionary.TryGetValue(checkPoint, out var buffer))
                    {
                        _inDimensionPointsList.Add(buffer);
                        continue;
                    }
                    var left = checkPoint.x - _halfNodeSize;
                    var right = checkPoint.x + _halfNodeSize;
                    var up = checkPoint.z + _halfNodeSize;
                    var down = checkPoint.z - _halfNodeSize;
                    var randomPointInCurrentNode = NoiseUtilsHelper.GetRandomPointInBounds(left, right, down, up);
                    var id = GameObjectPool.GetIdByProbability(prefabInfoList);
                    var cachedPoint = new CachedPointInfo(id, randomPointInCurrentNode);
                    _cachedPointsDictionary.Add(checkPoint, cachedPoint);
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

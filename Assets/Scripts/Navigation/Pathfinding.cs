using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Navigation
{
    public class Pathfinding : MonoBehaviour
    {
        /*private const int MoveDiagonalCost = 14;
        private const int MoveStraightCost = 10;
        
        public void FindPath(int2 startPosition, int2 endPosition, int dimension)
        {
            var gridSize = new int2(dimension, dimension);
            var pathNodeArray = InitPathNodeArray(dimension, endPosition);
            var neighbourOffsetArray = InitNeighbourOffsetArray();
            var endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, dimension);
            var startNode = InitStartNode(pathNodeArray, startPosition, dimension);
            
            var openList = new NativeList<int>(Allocator.Temp) {startNode.Index};
            var closedList = new NativeList<int>(Allocator.Temp);
            //openList.Add(startNode.Index);

            while (openList.Length > 0 )
            {
                var currentNodeIndex = GetLowestFCostNodeIndex(openList, pathNodeArray);
                var currentNode = pathNodeArray[currentNodeIndex];

                if (currentNodeIndex == endNodeIndex) break; // complete

                for (int i = 0; i < openList.Length; i++)
                {
                    if (openList[i] == currentNodeIndex)
                    {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }
                closedList.Add(currentNodeIndex);

                for (int i = 0; i < neighbourOffsetArray.Length; i++) // check neighbours
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.X + neighbourOffset.x, currentNode.Y + neighbourOffset.y);

                    if (!IsPositionInsideOfGrid(neighbourPosition, gridSize)) continue;

                    var neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, dimension);

                    if (closedList.Contains(neighbourNodeIndex)) continue;
                    
                    var neighbourNode = pathNodeArray[neighbourNodeIndex];

                    if (!neighbourNode.IsWalkable) continue;
                    
                    
                    
                }
            }


            neighbourOffsetArray.Dispose();
            pathNodeArray.Dispose();
            openList.Dispose();
            closedList.Dispose();
            
        }

        private static int CalculateIndex(int x, int y, int gridWidth)
        {
            return x + y * gridWidth;
        }
        private static int CalculateDistanceCost(int2 startPosition, int2 endPosition)
        {
            var xDistance = math.abs(startPosition.x - endPosition.x);
            var yDistance = math.abs(startPosition.y - endPosition.y);
            var remaining = math.abs(xDistance - yDistance);
            return MoveDiagonalCost * math.min(xDistance, yDistance) + MoveStraightCost * remaining;
        }
        private static int GetLowestFCostNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray)
        {
            PathNode lowestCostPathNode = pathNodeArray[openList[0]];
            for (int i = 0; i < openList.Length; i++)
            {
                PathNode testPathNode = pathNodeArray[openList[i]];
                if (testPathNode.FCost < lowestCostPathNode.FCost)
                {
                    lowestCostPathNode = testPathNode;
                }
            }
            return lowestCostPathNode.Index;
        }
        private static bool IsPositionInsideOfGrid(int2 gridPosition, int2 gridSize)
        {
            return gridPosition.x >= 0 &&
                   gridPosition.y >= 0 &&
                   gridPosition.x < gridSize.x &&
                   gridPosition.y < gridSize.y;
        }

        private NativeArray<PathNode> InitPathNodeArray(int dimension, int2 endPosition)
        {
            var pathNodeArray = new NativeArray<PathNode>(dimension*dimension, Allocator.Temp);
            for (int x = 0; x < dimension; x++)
            {
                for (int y = 0; y < dimension; y++)
                {
                    var pathNode = new PathNode
                    {
                        X = x,
                        Y = y,
                        Index = CalculateIndex(x, y, dimension),
                        GCost = int.MaxValue,
                        HCost = CalculateDistanceCost(new int2(x, y), endPosition),
                        IsWalkable = true,
                        PreviousNodeIndex = -1
                    };
                    pathNode.CalculateFCost();
                    pathNodeArray[pathNode.Index] = pathNode;

                }
            }

            return pathNodeArray;
        }
        private NativeArray<int2> InitNeighbourOffsetArray()
        {
            var neighbourOffsetArray = new NativeArray<int2>(
                new int2[]
                {
                    new int2(-1, 0), // Left
                    new int2(1, 0), // Right
                    new int2(0, 1), // Up
                    new int2(0, -1), // Down
                    new int2(-1, -1), // Left down
                    new int2(-1, +1), // Left up
                    new int2(+1, -1), // Right down
                    new int2(1, 1) // Right up
                }, Allocator.Temp);
            return neighbourOffsetArray;
        }

        private PathNode InitStartNode(NativeArray<PathNode> pathNodeArray, int2 startPosition, int dimension)
        {
            var startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, dimension)];
            startNode.GCost = 0;
            startNode.CalculateFCost();
            pathNodeArray[startNode.Index] = startNode;
        }
        private struct PathNode
        {
            public int X;
            public int Y;

            public int Index;
            public int PreviousNodeIndex;
            
            public int GCost;
            public int HCost;
            public int FCost;

            public bool IsWalkable;

            public void CalculateFCost()
            {
                FCost = GCost + HCost;
            }
        }*/
    }
}
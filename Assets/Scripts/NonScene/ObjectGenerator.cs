using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator<T> : MonoBehaviour where T : PrefabInfo
{
    [Header("Perlin noise settings")]
    [SerializeField] private float _noiseScale = 0.01f;
    private float _noiseOffsetX;
    private float _noiseOffsetY;
    protected float NoiseScale => _noiseScale; //DEBUG

    protected void SetNoiseOffsets()
    {
        _noiseOffsetX = Random.Range(-1000, 1000);
        _noiseOffsetY = Random.Range(-1000, 1000);
    }
    protected T GetById(List<T> prefabsInfo, int id)
    {
        foreach (T info in prefabsInfo)
        {
            if (info.Id == id) return info;
        }
        Debug.LogError("info with required id doesn't exist");
        return null;
    }
    private T GetRandomByProbability(List<T> prefabsInfo)
    {
        float probabilitySum = 0;
        float randomPoint;

        foreach (T info in prefabsInfo)
        {
            probabilitySum += info.Probability;
        }
        randomPoint = Random.value * probabilitySum;

        foreach(T info in prefabsInfo)
        {
            if (randomPoint < info.Probability) return info;
            else randomPoint -= info.Probability;
        }
        return prefabsInfo[prefabsInfo.Count - 1];
    }
    protected T GetByPerlinNoiseAtPosition(List<T> prefabsInfo, Vector3 position, bool returnNullAtError = true)
    {
        List<T> prefabsInfoToReturn = new List<T>();
        Vector3 noiseWorldPosition = position * _noiseScale + new Vector3(_noiseOffsetX, 0, _noiseOffsetY);
        float noiseValue = Mathf.Clamp01(Mathf.PerlinNoise(noiseWorldPosition.x, noiseWorldPosition.z));
        foreach (T info in prefabsInfo)
        {
            if (noiseValue >= info.MinPerlinValue && noiseValue <= info.MaxPerlinValue)
            {
                prefabsInfoToReturn.Add(info);
            }
        }
        if (prefabsInfoToReturn.Count == 0)
        {
            if (returnNullAtError) return null;
            else
            {
                T prefabInfoToReturn = null;
                float minAveragePerlinValue = 1f;

                foreach(T info in prefabsInfo)
                {
                    float averagePerlinValue = Mathf.Abs((info.MinPerlinValue + info.MaxPerlinValue) / 2f - noiseValue);
                    if (averagePerlinValue < minAveragePerlinValue) prefabInfoToReturn = info;
                }
                return prefabInfoToReturn;
            }
        }
        return GetRandomByProbability(prefabsInfoToReturn);
    }
    protected Vector3 GetRandomPoint(float left, float right, float buttom, float up)
    {
        float x = Random.Range(left, right);
        float z = Random.Range(buttom, up);
        return new Vector3(x, 0, z);
    }
    protected Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    //ONLY DEBUG
    protected Color OnlyDebugCalculateColor(int x, int y, int width, int height, float chunkSize, Vector3 position)
    {
        float xCoord = x * chunkSize / width * _noiseScale + _noiseOffsetX + position.x;
        float yCoord = y * chunkSize / height * _noiseScale + _noiseOffsetY + position.z;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
}

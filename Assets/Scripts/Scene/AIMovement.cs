using System.Collections;
using UnityEngine;
using Pathfinding;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _distanceToNextWaypoint = 1f;

    private Path _path;
    private Rigidbody _rigidbody;
    private Seeker _seeker;

    private int _nextWayPoint = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _seeker = GetComponent<Seeker>();
    }
    private Vector3 GetDirection() => (_path.vectorPath[_nextWayPoint] - transform.position).normalized;
    private float GetAngle() => Vector3.SignedAngle(transform.forward, GetDirection(), Vector3.up);
    private float GetDistance() => Vector3.Distance(transform.position, _path.vectorPath[_nextWayPoint]);

    private IEnumerator MoveToTargerRoutine()
    {
        while (true)
        {
            if (_path == null) yield break;

            if (_nextWayPoint >= _path.vectorPath.Count)
            {
                _rigidbody.velocity = Vector3.zero;
                yield break;
            }
            _rigidbody.velocity = GetDirection() * _speed;
            if (GetDistance() < _distanceToNextWaypoint)
            {
                _nextWayPoint++;
                if (_nextWayPoint < _path.vectorPath.Count)
                {
                    transform.Rotate(Vector3.up, GetAngle());
                }
            }
            yield return null;
        }

    }
    public void SetDestination(Vector3 destination)
    {
        _seeker.StartPath(transform.position, destination, OnPathComplete);
    }
    private void OnPathComplete(Path path)
    {
        if (path.error) Debug.LogError("Error with calculating path. " + path.error);
        if(!path.error)
        {
            _path = path;
            _nextWayPoint = 0;
            DrawPoints();
            StartCoroutine(MoveToTargerRoutine());
        }
    }

    private void DrawPoints()
    {
        Debug.Log("point count: " + _path.vectorPath.Count);
        foreach(Vector3 point in _path.vectorPath)
        {
            Debug.DrawLine(point, point + Vector3.up * 2, Color.black, 1);
        }
    }
}

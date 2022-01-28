using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Components;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform offsetPoint;
    [SerializeField] private float smoothTime = 0.25f;

    private Transform _target;
    private Vector3 _offset;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _desiredPosition;
    private void Start()
    {
        _offset = transform.position - offsetPoint.position;
        _desiredPosition = offsetPoint.position;
        StartCoroutine(FollowTargetCoroutine());
    }
    private IEnumerator FollowTargetCoroutine()
    {
        yield return StartCoroutine(FindTargetCoroutine());

        while (true)
        {
            if (_target.gameObject.activeSelf)
            {
                _desiredPosition = _target.position + _offset;
            }
            var smoothedPosiiton = Vector3.SmoothDamp(transform.position, _desiredPosition, ref _velocity, smoothTime);
            transform.position = smoothedPosiiton;

            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator FindTargetCoroutine()
    {
        while (true)
        {
            foreach (Movement player in FindObjectsOfType<Movement>())
            {
                if (player.GetComponent<PhotonView>().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    _target = player.transform;
                    yield break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}

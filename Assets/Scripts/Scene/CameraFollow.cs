using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _offsetPoint;
    [SerializeField] private float _smoothTime = 0.4f;

    private Transform _target;
    private Vector3 _offset;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _desiredPosition;
    private void Start()
    {
        _offset = transform.position - _offsetPoint.position;
        _desiredPosition = _offsetPoint.position;
        StartCoroutine(FollowTargetRoutine());
    }
    private IEnumerator FollowTargetRoutine()
    {
        yield return StartCoroutine(FindTargetRoutine());

        while(true)
        {
            if (_target.gameObject.activeSelf) _desiredPosition = _target.position + _offset;
            Vector3 smoothedPosiiton = Vector3.SmoothDamp(transform.position, _desiredPosition, ref _velocity, _smoothTime);
            transform.position = smoothedPosiiton;

            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator FindTargetRoutine()
    {
        while (true)
        {
            foreach (PlayerMovement player in FindObjectsOfType<PlayerMovement>())
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

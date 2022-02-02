using System.Collections;
using System.Collections.Generic;
using Core;
using Photon.Pun;
using UnityEngine;

namespace Character
{
    public abstract class BaseCameraFollow : MonoBehaviour
    {
        protected Transform Target;
        private void OnEnable()
        {
            PlayerObserver.OnPlayersFound.AddListener(OnPlayerFound);
        }
        protected abstract IEnumerator FollowTargetCoroutine();
        private void OnPlayerFound(List<Transform> transforms)
        {
            foreach(Transform player in transforms)
            {
                if (player.GetComponent<PhotonView>().OwnerActorNr != PhotonNetwork.LocalPlayer.ActorNumber) continue;
                Target = player.transform;
                StartCoroutine(FollowTargetCoroutine());
                return;
            }
        }
    }
}
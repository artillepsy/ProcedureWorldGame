using System.Collections;
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
        private void OnPlayerFound()
        {
            foreach(Transform player in PlayerObserver.FoundPlayers)
            {
                if (player.GetComponent<PhotonView>().OwnerActorNr != PhotonNetwork.LocalPlayer.ActorNumber) continue;
                Target = player.transform;
                StartCoroutine(FollowTargetCoroutine());
                return;
            }
        }
    }
}
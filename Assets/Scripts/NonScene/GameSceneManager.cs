using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform spawner;
    [SerializeField] private GameObject playerPrefab;
    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawner.position, Quaternion.identity);
    }
    void Update()
    {
        
    }
    // when player left room scene manager (master client) destroys player's prefab
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        foreach (PlayerMovement player in FindObjectsOfType<PlayerMovement>())
        {
            if (player.GetComponent<PhotonView>().OwnerActorNr.Equals(otherPlayer.ActorNumber))
            {
                PhotonNetwork.Destroy(player.gameObject);
                return;
            }
        }
    }

}

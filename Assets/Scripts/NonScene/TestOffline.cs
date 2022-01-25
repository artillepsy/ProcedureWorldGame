using Photon.Pun;
using UnityEngine;

public class TestOffline : MonoBehaviourPunCallbacks
{
    [SerializeField] private bool OfflineMode = true;
    void Awake()
    {
        if (OfflineMode)
        {
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.CreateRoom("test");
        }
    }

}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncActiveStatus : MonoBehaviourPun
{
    PhotonView pv;
    private void Awake()
    {
        pv = PhotonView.Get(this);
    }

    [PunRPC]
    public void SetActiveStatus(bool status)
    {
        gameObject.SetActive(status);
    }
    private void OnEnable()
    {
        if (pv) pv.RPC(nameof(SetActiveStatus), RpcTarget.Others, true);
    }
    private void OnDisable()
    {
        if (pv) pv.RPC(nameof(SetActiveStatus), RpcTarget.Others, false);
    }
}

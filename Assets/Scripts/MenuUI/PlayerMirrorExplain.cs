using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMirrorExplain : MonoBehaviour
{
    public int team;
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            if (team != 3)
                gameObject.SetActive(false);
        }
    }
}

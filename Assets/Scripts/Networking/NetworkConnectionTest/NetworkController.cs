using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //Connects to Photon Master Servers
        //Read the Documentation of GrensControle to find the documentation of Photon. 
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }
}

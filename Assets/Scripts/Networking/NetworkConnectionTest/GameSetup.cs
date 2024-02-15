using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    private PhotonView _photonView;
    void Start()
    {
        CreatePlayer();
       // CreateEnviroment();
    }

    private void CreatePlayer()
    {
       
            var position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-2, 5));
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), position, Quaternion.identity);
     
    }

   /* private void CreateEnviroment()
    {
        var position = new Vector3(5, -3, 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Plane"), position, Quaternion.identity);
    } */
}

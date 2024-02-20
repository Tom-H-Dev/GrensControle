using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnpoints = new List<Transform>();
    void Start()
    {
        CreatePlayer();
        // CreateEnviroment();
    }

    private void CreatePlayer()
    {

        var position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-2, 5));
        GameObject l_player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), position, Quaternion.identity);
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            l_player.transform.position = _spawnpoints[i].position;
        }

    }

    /* private void CreateEnviroment()
     {
         var position = new Vector3(5, -3, 0);
         PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Plane"), position, Quaternion.identity);
     } */
}

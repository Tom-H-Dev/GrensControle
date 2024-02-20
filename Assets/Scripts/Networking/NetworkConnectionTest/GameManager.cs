using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnpoints = new List<Transform>();
    [SerializeField] private GameObject _playerPrefab;
    private int spawnIndex;
    void Start()
    {
        CreatePlayer();
        // CreateEnviroment();
    }

    private void CreatePlayer()
    {

        //var position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-2, 5));
        //GameObject l_player = PhotonNetwork.Instantiate(_playerPrefab.name, position, Quaternion.identity);

        if (spawnIndex >= _spawnpoints.Count) spawnIndex = 0;
        Vector3 l_position = _spawnpoints[spawnIndex].transform.position;

        GameObject newPlayerObject = PhotonNetwork.Instantiate(_playerPrefab.name, l_position, Quaternion.identity);
        spawnIndex++;
    }

    /* private void CreateEnviroment()
     {
         var position = new Vector3(5, -3, 0);
         PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Plane"), position, Quaternion.identity);
     } */
}

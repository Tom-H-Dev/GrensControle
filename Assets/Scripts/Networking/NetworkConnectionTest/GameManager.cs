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
    }

    private void CreatePlayer()
    {
        switch (PhotonNetwork.LocalPlayer.ActorNumber)
        {
            case 1:
                PhotonNetwork.Instantiate(_playerPrefab.name, _spawnpoints[0].position, Quaternion.identity);
                break;
            case 2:
                PhotonNetwork.Instantiate(_playerPrefab.name, _spawnpoints[1].position, Quaternion.identity);
                break;
            case 3:
                PhotonNetwork.Instantiate(_playerPrefab.name, _spawnpoints[2].position, Quaternion.identity);
                break;
        }
    }
}

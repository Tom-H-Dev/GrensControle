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
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate(_playerPrefab.name, _spawnpoints[0].position, Quaternion.identity);
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            PhotonNetwork.Instantiate(_playerPrefab.name, _spawnpoints[1].position, Quaternion.identity);
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
        {
            PhotonNetwork.Instantiate(_playerPrefab.name, _spawnpoints[2].position, Quaternion.identity);
        }
    }
}

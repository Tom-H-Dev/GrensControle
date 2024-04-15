using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("The spawn positions for the player when they spawn into the game.")]
    [SerializeField] private List<Transform> _spawnpoints = new List<Transform>();
    [Tooltip("The prefab for the player.")]
    [SerializeField] private GameObject _playerPrefab;
    [Tooltip("The Animator of the canvas")]
    public Animator _canvasAnimator;
    public static GameManager instance;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        CreatePlayer();
    }

    /// <summary>
    /// Sets the different players to the correct spawn positions.
    /// </summary>
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

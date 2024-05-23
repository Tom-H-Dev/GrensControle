using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private Transform _carSpawnLocation;

    [SerializeField] private GameObject _carPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && PhotonNetwork.IsMasterClient)
        {
            print("spawn");
            PhotonNetwork.Instantiate(_carPrefab.name, _carSpawnLocation.position, Quaternion.identity);
        }
    }
}

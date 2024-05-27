using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private Transform _carSpawnLocation;

    [SerializeField] private GameObject _carPrefab;
    public int _currentVehiclesInt;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && PhotonNetwork.IsMasterClient)
        {
            print("spawn");
            SpawnVehicles();
        }
    }

    private void SpawnVehicles()
    {
        PhotonNetwork.Instantiate(_carPrefab.name, _carSpawnLocation.position, Quaternion.identity);
        _currentVehiclesInt++;
    }

    private IEnumerator SpawnTimer()
    {
        SpawnVehicles();
        int l_t = Random.Range(15, 30);
        yield return new WaitForSeconds(l_t);
        StartCoroutine(SpawnTimer());
    }
}

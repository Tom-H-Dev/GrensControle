using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private Transform _carSpawnLocation;

    [SerializeField] private GameObject _carPrefab;
    public int _currentVehiclesInt;
    private bool once = false;


    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!once)
            {
                PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
                if (players.Length >= 3)
                {
                    once = true;
                    StartCoroutine(SpawnTimer());
                }
            }
        }

    }

    private void SpawnVehicles()
    {
        PhotonNetwork.Instantiate(_carPrefab.name, _carSpawnLocation.position, Quaternion.identity);
        _currentVehiclesInt++;
    }

    private IEnumerator SpawnTimer()
    {
        if (RouteManager.instance._activeCars.Count < RouteManager.instance._maximumVehicles)
        {
            SpawnVehicles();
            int l_t = Random.Range(30, 45);
            yield return new WaitForSeconds(l_t);
            StartCoroutine(SpawnTimer());
        }
        else StartCoroutine(CooldownTimer());
    }
    private IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(SpawnTimer());
    }
}

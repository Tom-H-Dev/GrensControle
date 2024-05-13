using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VehicleManager : MonoBehaviour
{
    public int _maxVehicles;
    public int _currentVehiclesInt;
    [SerializeField] GameObject _carPrefab;
    [SerializeField] Transform _carSpawnLocation;
    [SerializeField] BarrierManager _entranceBarrier;
    public List<GameObject> _currentVehicles;
    public Transform insideBaseLocation;
    public bool photonServer;
    void Start()
    {
        StartCoroutine(SpawnTimer());
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SpawnVehicle();
        }
    }

    private void SpawnVehicle()
    {
        if (_currentVehicles.Count < _maxVehicles)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //print("spawning vehicle in server...");
                GameObject _currentVehicle = PhotonNetwork.Instantiate(_carPrefab.name, _carSpawnLocation.position, Quaternion.identity);
                _entranceBarrier.AddToQueue(_currentVehicle.GetComponent<CarBehaviour>());
                _currentVehicles.Add(_currentVehicle);
                _currentVehiclesInt++;
            }
            else if (photonServer)
            {
                print("spawning vehicle locally...");
                GameObject _currentVehicle = Instantiate(_carPrefab, _carSpawnLocation.position, Quaternion.identity);
                _entranceBarrier.AddToQueue(_currentVehicle.GetComponent<CarBehaviour>());
                _currentVehicles.Add(_currentVehicle);
                _currentVehiclesInt++;
            }
        }
    }

    private IEnumerator SpawnTimer()
    {
        SpawnVehicle();
        int l_t = Random.Range(20,40);
        yield return new WaitForSeconds(l_t);
        StartCoroutine(SpawnTimer());
    }
}

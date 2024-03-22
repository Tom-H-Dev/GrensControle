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
    [SerializeField] List<GameObject> _currentVehicles;
    public Transform insideBaseLocation;
    void Start()
    {
        SpawnVehicle();
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
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject _currentVehicle = PhotonNetwork.Instantiate(_carPrefab.name, _carSpawnLocation.position, Quaternion.identity);
            _entranceBarrier.GetStoppingSpot(_currentVehicle.GetComponent<CarBehaviour>());
            //_currentVehicle.name = (_currentVehicleNumber = +1).ToString();
            //_currentVehicles.Add(_currentVehicle);
            _currentVehiclesInt++;
            _entranceBarrier._queue[_currentVehiclesInt - 1] = _currentVehicle.GetComponent<CarBehaviour>();
        }
    }
}

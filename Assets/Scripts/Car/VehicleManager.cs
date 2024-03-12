using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public int _maxVehicles;
    [SerializeField] GameObject _carPrefab;
    [SerializeField] Transform _carSpawnLocation;

    void Start()
    {
        StartCoroutine(SpawnVehicle());
    }

    
    void Update()
    {
        
    }

    private IEnumerator SpawnVehicle()
    {
        int _currentVehicleNumber = 0;
        GameObject _currentVehicle = Instantiate(_carPrefab, _carSpawnLocation.position, Quaternion.identity);
        _currentVehicle.GetComponent<CarBehaviour>()._currentTarget = _carSpawnLocation.transform;
        _currentVehicle.name = (_currentVehicleNumber = +1).ToString();
        yield return null;
    }
}

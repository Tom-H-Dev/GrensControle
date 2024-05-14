using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private Transform _carSpawnLocation;

    [SerializeField] private GameObject _carPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("spawn");
            Instantiate(_carPrefab, _carSpawnLocation.position, Quaternion.identity);
        }
    }
}

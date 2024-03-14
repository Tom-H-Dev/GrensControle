using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrabandManager : MonoBehaviour
{
    [SerializeField] List<Transform> _contrabandObjects = new List<Transform>();
    [SerializeField] List<Transform> _contrabandLocations = new List<Transform>();
    [SerializeField] GameObject[] _currentContrabandInsideVehicle;
    [SerializeField] [Range(0, 100)] float contrabandChance;

    private void Start()
    {
        
    }

}

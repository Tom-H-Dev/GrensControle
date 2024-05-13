using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrabandManager : MonoBehaviour
{
    [SerializeField] List<Transform> _contrabandObjects = new List<Transform>();
    [SerializeField] List<Transform> _contrabandLocations = new List<Transform>();
    [SerializeField] GameObject[] _currentContrabandInsideVehicle;
    [SerializeField] [Range(0, 100)] float contrabandChance;
    [SerializeField][Range(0, 100)] float multipleContrabandChance;
    public bool _hasContraband;

    private void Start()
    {
        int randomContrabandChance = Random.Range(0, 100);
        if (randomContrabandChance < contrabandChance)
        {
            //print(gameObject.name + " Has contraband");
            for (int i = 0; i < _contrabandLocations.Count; i++)
            {
                randomContrabandChance = Random.Range(0, 100);
                if (randomContrabandChance < multipleContrabandChance)
                {
                    GameObject randomContrabandObject = _contrabandObjects[Random.Range(0, _contrabandObjects.Count)].gameObject;

                    Instantiate(randomContrabandObject, _contrabandLocations[i].position, randomContrabandObject.transform.rotation, _contrabandLocations[i]);
                }
            }
            _hasContraband = true;
        }
        else _hasContraband = false;
    }

}

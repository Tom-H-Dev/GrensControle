using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.AI;
using Unity.VisualScripting;
using System.ComponentModel;
using Unity.Collections;
using System.Net.NetworkInformation;

public class CarBehaviour : MonoBehaviour
{
    [Header("Voertuig Data")]
    public bool _isMillitairyVehicle; // Will add DM in the license palte
    public bool _hasDutchLicensePlate; //WIll make the license plate color yellow
    public string _duplicateCode = null; // The little number on how often the driver has lost their vehicle, purely aesthetical and has no fucntion
    public string[] _landCodes; // Such as NL (netherlands), PL (poland), DE (Germany) etc...
    public string _landCode;
    public string _licensePlate;
    [SerializeField] LicensePlateManager[] _licensePlates;
    [Header("Vehicle dynamics")]
    NavMeshAgent _agent;
    [SerializeField] GameObject[] _wheels; //LF, RF, LB, RB
    [SerializeField] GameObject _currentTarget; // The car will target this object
    [SerializeField] float _normalSpeed; // The default speed of the car
    [SerializeField] AudioSource _honkSound; //Honk sound effect
    [SerializeField] AudioSource _brakeSound; //Brake screetch sound effect
    [SerializeField] LayerMask _CollisionLayerMask; //COllision layermask for the emergency brake
    bool _emergencyBrake; // Bool that keeps track of braking
    [Header("Radius")]
    [SerializeField] float _slowingRadius; // In this radius the car will half it's normal speed (_normalSpeed)
    [SerializeField] float _brakingRadius; // in this radius the car will drive 1/3rd of it's normal speed (_normalSpeed)
    [SerializeField] float _stoppingRadius; // In this radius the car will stop the vehicle
    [SerializeField] Vector3 emergencyBreakRadius; // this radius is the dimensions of the braking speed
    [SerializeField] GameObject emergencyBreakPos; // a gameobject child that is the location of the emergency brake radius
    void Start()
    {
        _emergencyBrake = false;
        string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // alphabet....
        string _middleText = null;
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _normalSpeed;
        if (_isMillitairyVehicle )
        { 
        _middleText = "DM" + _alphabet[Random.Range(0, _alphabet.Length)]; // Add DM into the license plate in case it's a dutch militairy vehicle
        }
        else
        {
            _middleText = _alphabet[Random.Range(0, _alphabet.Length)].ToString() + _alphabet[Random.Range(0, _alphabet.Length)].ToString() + _alphabet[Random.Range(0, _alphabet.Length)].ToString();
        }

        _licensePlate = Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString() + "-" + _middleText + "-" + Random.Range(0, 9).ToString();

        float a = Random.value;
        if (a < 0.05f)
        {
            _duplicateCode = "1";
        }

        float b = Random.value; 
        if (b < 0.50f)
        {
            print(b);
            _hasDutchLicensePlate = false;
            _landCode = _landCodes[Random.Range(1, _landCodes.Length)];
        }
        else
        {
            _hasDutchLicensePlate = true;
            _landCode = _landCodes[0];
        }

        for (int i = 0; i < _licensePlates.Length; i++)
        {
            _licensePlates[i]._licenseText.text = _licensePlate;
            _licensePlates[i]._landCodeText.text = _landCode;
            _licensePlates[i]._duplicateText.text = _duplicateCode;

            if (_hasDutchLicensePlate)
            {
                print("yellow plate");
                Material[] materials = _licensePlates[i].GetComponent<MeshRenderer>().materials;
                materials[3] = _licensePlates[i]._yellowPlate;
                materials[4] = _licensePlates[i]._yellowPlate;
                _licensePlates[i].GetComponent<MeshRenderer>().materials = materials;
            }
            else if (!_hasDutchLicensePlate)
            {
                print("White plate");
                Material[] materials = _licensePlates[i].GetComponent<MeshRenderer>().materials;
                materials[3] = _licensePlates[i]._whitePlate;
                materials[4] = _licensePlates[i]._whitePlate;
                _licensePlates[i].GetComponent<MeshRenderer>().materials = materials;
            }

        }
    }

    void Update()
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, _slowingRadius, _CollisionLayerMask);
        _agent.SetDestination(_currentTarget.transform.position);
        _agent.stoppingDistance = _stoppingRadius;
        float agentToFinishDistance = Vector3.Distance(transform.position, _currentTarget.transform.position);

        if (agentToFinishDistance <= _slowingRadius && agentToFinishDistance > _brakingRadius)
        {
            _agent.speed = _normalSpeed * 0.5f;

        }
        else if (agentToFinishDistance <= _brakingRadius && agentToFinishDistance > _stoppingRadius)
        {
            _agent.speed = _normalSpeed * 0.3f;
        }
        else if (agentToFinishDistance < _stoppingRadius)
        {
            _agent.speed = 0;
        }
        else
        {
            //_agent.speed = _normalSpeed;
        }

        for (int i = 0; i < nearbyColliders.Length; i++)
        {
            float agentToObjectDistance = Vector3.Distance(transform.position, nearbyColliders[i].transform.position);

            if (nearbyColliders[i].gameObject.layer == 3)
            {

                if (agentToObjectDistance <= _slowingRadius && agentToObjectDistance > _brakingRadius)
                {
                    if (agentToFinishDistance < Vector3.Distance(_currentTarget.transform.position, nearbyColliders[i].transform.position))
                    {
                        print("Slowing down " + gameObject.name);
                        _agent.speed = _normalSpeed * 0.5f;
                    }
                }
                else if (agentToObjectDistance < _brakingRadius)
                {
                    print("Test ");
                    if (agentToFinishDistance < Vector3.Distance(_currentTarget.transform.position, nearbyColliders[i].transform.position))
                    {
                        print("Stopping " + gameObject.name);
                        _agent.speed = 0;
                    }
                }

            }     
        }

        //Collider[] colliders = Physics.OverlapBox(emergencyBreakPos.transform.position, emergencyBreakRadius / 2, Quaternion.identity, _CollisionLayerMask);
        //{
        //    if (colliders.Length > 0)
        //    {
        //        if (!_emergencyBrake)
        //        {
        //            print("Braking!");
        //            _emergencyBrake = true;
        //            _agent.speed = 0;
        //            _brakeSound.Play();
        //            _honkSound.Play();
        //        }
        //    }
        //    else
        //    {
        //        _emergencyBrake = false;
        //    }
           
        //}

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _slowingRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _brakingRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _stoppingRadius);
        Gizmos.DrawWireCube(emergencyBreakPos.transform.position, emergencyBreakRadius);
    }

    public void NextStopPoint(GameObject nextStop)
    {
        _currentTarget = nextStop;
    }
}

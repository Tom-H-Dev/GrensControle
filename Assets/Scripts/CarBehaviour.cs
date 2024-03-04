using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.AI;
using Unity.VisualScripting;

public class CarBehaviour : MonoBehaviour
{
    [Header("Voertuig Data")]
    public bool _isMillitairyVehicle;
    public bool _hasDutchLicensePlate;
    public string _duplicateCode = null;
    public string _landCode;
    public string _licensePlate;
    [SerializeField] LicensePlateManager[] _licensePlates;
    [Header("Vehicle dynamics")]
    NavMeshAgent _agent;
    [SerializeField] GameObject[] _wheels; //LF, RF, LB, RB
    [SerializeField] Transform[] _stopLocations;
    [SerializeField] GameObject _currentTarget;
    [SerializeField] float _normalSpeed;
    [SerializeField] AudioSource _honkSound;
    [SerializeField] AudioSource _brakeSound;
    [SerializeField] LayerMask _CollisionLayerMask;
    bool _emergencyBrake;
    [Header("Radius")]
    [SerializeField] float _slowingRadius;
    [SerializeField] float _brakingRadius;
    [SerializeField] float _stoppingRadius;
    void Start()
    {
        _emergencyBrake = false;
        string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string _middleText = null;
        _agent = GetComponent<NavMeshAgent>();
        if (_isMillitairyVehicle )
        { 
        _middleText = "DM" + _alphabet[Random.Range(0, _alphabet.Length)]; // Voeg DM toe omdat het een militair voertuig is
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
        }
        else
        {
            _hasDutchLicensePlate = true;
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

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(_currentTarget.transform.position);
        _agent.stoppingDistance = _stoppingRadius;

        float agentToFinishDistance = Vector3.Distance(transform.position, _currentTarget.transform.position);
<<<<<<< HEAD
        //print(agentToFinishDistance);
=======
>>>>>>> dev-tom

        if (!_emergencyBrake)
        {
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
                _agent.speed = _normalSpeed;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _slowingRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _brakingRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _stoppingRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Hit something!");
        //if (collision.gameObject.layer == _CollisionLayerMask)
        //{
            print("Hit something!");
            _emergencyBrake = true;
            _agent.speed = 0;
        _brakeSound.Play();
        _honkSound.Play();
        //}
    }
}

using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class CarBehaviour : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    [Header("Voertuig Data")]
    public bool _isMillitairyVehicle; // Will add DM in the license palte
    public bool _hasDutchLicensePlate; //WIll make the license plate color yellow
    public string _duplicateCode = null; // The little number on how often the driver has lost their vehicle, purely aesthetical and has no fucntion
    [SerializeField] string[] _landCodes; // Such as NL (netherlands), PL (poland), DE (Germany) etc...
    public string _landCode;
    public string _licensePlate;
    [SerializeField] LicensePlateManager[] _licensePlates;
    public bool _override =false;
    [SerializeField] private AudioMixer _carMixer;
    //----------------------------------------------------------------------------------------------------
    [Header("Vehicle dynamics")]
    NavMeshAgent _agent; //NavMesh agent
    [SerializeField] GameObject[] _wheels; //LF, RF, LB, RB
    public Transform _currentTarget; // The car will target this object
    [SerializeField] float _defaultSpeed; // The default speed of the car
    public float _defaultAngularSpeed;
    [SerializeField] AudioSource _honkSound; //Honk sound effect
    [SerializeField] AudioSource _brakeSound; //Brake screetch sound effect
    [SerializeField] LayerMask _CollisionLayerMask; //COllision layermask for the emergency brake
    bool _emergencyBrake; // Bool that keeps track of braking
    //----------------------------------------------------------------------------------------------------
    [Header("Radius")]
    [SerializeField] float _slowingRadius; // In this radius the car will half it's normal speed (_normalSpeed)
    [SerializeField] float _brakingRadius; // in this radius the car will drive 1/3rd of it's normal speed (_normalSpeed)
    public float _stoppingRadius; // In this radius the car will stop the vehicle
    [SerializeField] Vector3 emergencyBreakRadius; // this radius is the dimensions of the braking speed
    [SerializeField] GameObject emergencyBreakPos; // a gameobject child that is the location of the emergency brake radius
    public float agentToFinishDistance;
    //----------------------------------------------------------------------------------------------------
    void Start()
    {

        Physics.IgnoreLayerCollision(3, 15);
        _emergencyBrake = false;
        string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // alphabet....
        string _middleText = null;
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _defaultSpeed;

        if (PhotonNetwork.IsMasterClient || _override)
        {
            float a = Random.value;
            if (a < 0.05f)
            {
                _duplicateCode = "1";
            }

            float b = Random.value;
            if (b < 0.50f)
            {
                _hasDutchLicensePlate = false;
                _landCode = _landCodes[Random.Range(1, _landCodes.Length)];
            }
            else
            {
                _hasDutchLicensePlate = true;
                _landCode = _landCodes[0];

                float c = Random.value;
                if (c < 0.30f)
                {
                    _isMillitairyVehicle = true;
                }
            }

            if (_isMillitairyVehicle)
            {
                _middleText = "DM" + _alphabet[Random.Range(0, _alphabet.Length)]; // Add DM into the license plate in case it's a dutch militairy vehicle
            }
            else
            {
                _middleText = _alphabet[Random.Range(0, _alphabet.Length)].ToString() + _alphabet[Random.Range(0, _alphabet.Length)].ToString() + _alphabet[Random.Range(0, _alphabet.Length)].ToString();
            }

            _licensePlate = Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString() + "-" + _middleText + "-" + Random.Range(0, 9).ToString(); // set the license plate



            for (int i = 0; i < _licensePlates.Length; i++)
            {
                _licensePlates[i]._licenseText.text = _licensePlate;
                _licensePlates[i]._landCodeText.text = _landCode;
                _licensePlates[i]._duplicateText.text = _duplicateCode;

                if (_hasDutchLicensePlate)
                {
                    Material[] materials = _licensePlates[i].GetComponent<MeshRenderer>().materials;
                    materials[3] = _licensePlates[i]._yellowPlate;
                    materials[4] = _licensePlates[i]._yellowPlate;
                    _licensePlates[i].GetComponent<MeshRenderer>().materials = materials;
                }
                else if (!_hasDutchLicensePlate)
                {
                    Material[] materials = _licensePlates[i].GetComponent<MeshRenderer>().materials;
                    materials[3] = _licensePlates[i]._whitePlate;
                    materials[4] = _licensePlates[i]._whitePlate;
                    _licensePlates[i].GetComponent<MeshRenderer>().materials = materials;
                }

            }
        }
    }

    void Update()
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, _slowingRadius, _CollisionLayerMask);
        _agent.SetDestination(_currentTarget.position);
        _agent.stoppingDistance = _stoppingRadius;
        agentToFinishDistance = Vector3.Distance(transform.position, _currentTarget.position);

        if (agentToFinishDistance <= _slowingRadius && agentToFinishDistance > _brakingRadius)
        {
            _agent.speed = _defaultSpeed * 0.5f;
            //GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.SetFloat("Pitch", 165);
            _carMixer.SetFloat("MyExposedParam", 1.65f);
        }
        else if (agentToFinishDistance <= _brakingRadius && agentToFinishDistance > _stoppingRadius)
        {
            _agent.speed = _defaultSpeed * 0.3f;
            //GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.SetFloat("Pitch", 135);
            _carMixer.SetFloat("MyExposedParam", 1.3f);
        }
        else if (agentToFinishDistance < _stoppingRadius)
        {
            _agent.speed = 0;
            //GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.SetFloat("Pitch", 100);
            _carMixer.SetFloat("MyExposedParam", 1);
        }
        else
        {
            _agent.speed = _defaultSpeed;
            //GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.SetFloat("Pitch", 200);
            _carMixer.SetFloat("MyExposedParam", 2);
        }

        Vector3 vehicleVelocity = gameObject.GetComponent<Rigidbody>().velocity;

        foreach (GameObject wheel in _wheels)
        {
            Vector3 velocity = gameObject.GetComponent<Rigidbody>().velocity;
            float rotationAmount = velocity.magnitude * Time.deltaTime * Mathf.Rad2Deg;
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
}

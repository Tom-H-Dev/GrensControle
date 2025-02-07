using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public enum CarStates
{
    arriving,
    accepted,
    declined,
    queuing,
    inQueue
};

public class CarAI : MonoBehaviourPun
{
    public CarStates _carState = CarStates.arriving;

    [Header("Paths")]
    [SerializeField] private List<Transform> _nodes;
    public int _currentNode = 0;

    [Header("Wheels")]
    public float _maxSteerAngle = 45f;
    public WheelCollider _wheelFL;
    public WheelCollider _wheelFR;
    public WheelCollider _wheelRL;
    public WheelCollider _wheelRR;

    [Header("Engine")]
    public float _maxMotorTorque = 70f;
    public float _curSpeed;
    public float _maxSpeed = 80f;
    public bool _isBraking = false;
    public float _maxBrakeTorque = 150f;
    public bool _emergencyBrake;
    public bool _isMovingBackwards = false;
    public bool _movingToQuePoint = false;


    [Header("Sensors")]
    public float _sensorLength = 5f;
    public Vector3 _fronstSensorPosition = new Vector3(0, 0.5f, 2);
    public float _frontSideSensorPos = 0.2f;
    public float _frontSensorAngle = 30f;
    [SerializeField] private List<GameObject> _sensorObjects;
    [SerializeField] private List<GameObject> _sensorLookObjects;
    [SerializeField] private CollisionChecker _collisionChecker;

    [Header("Data")]
    public bool _isMilitaryVehicle; // Will add DM in the license palte
    public bool _hasDutchLicensePlate; //WIll make the license plate color yellow
    public string _duplicateCode = null; // The little number on how often the driver has lost their vehicle, purely aesthetical and has no function
    [SerializeField] string[] _landCodes; // Such as NL (netherlands), PL (poland), DE (Germany) etc...
    public string _landCode;
    public string _licensePlate;
    [SerializeField] LicensePlateManager[] _licensePlates;
    [SerializeField] private Material _yellowPlate;
    [SerializeField] private Material _whitePlate;
    public bool _isControlable = false;
    public bool _hasBeenChecked = false;
    private BarrierManager _barrierManager;
    [SerializeField] private Vector3 _com;
    public bool _arriving = true;
    public int _waitingIndex;
    public int _backupCurrentNode;
    bool _waitForFrame = false;
    [SerializeField] private AudioSource _carAudioSource;
    [SerializeField] private float _baseEnginePitch;
    [SerializeField] private float _enginePitchMultiplier;
    private List<bool> _sensorHits = new List<bool> { false, false, false, false, false, false, false, false, false };

    [Header("Network")]
    public bool _override = false;
    public bool inQueue = false;
    private PhotonView _photonView;

    [Header("Animations")]
    private const string _enterBase = "EnterBase";
    private const string _denyBase = "DenyBase";
    private const string _move21 = "Move21";
    private const string _move32 = "Move32";
    private const string _move43 = "Move43";
    private const string _move54 = "Move54";
    private const string _movePos1 = "MovePos1";
    private const string _movePos2 = "MovePos2";
    private const string _movePos3 = "MovePos3";
    private const string _movePos4 = "MovePos4";
    private const string _movePos5 = "MovePos5";

    public int carIndex;
    public int _raycastRange = 4;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _barrierManager = FindObjectOfType<BarrierManager>();
        Physics.IgnoreLayerCollision(3, 15);
        GetComponent<Rigidbody>().centerOfMass = _com;
        _nodes = new List<Transform>();
        string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // alphabet....
        string _middleText = null;
        //_photonView.RPC("CloseDoors", RpcTarget.AllBufferedViaServer);
        if (PhotonNetwork.IsMasterClient || _override)
        {
            photonView.RPC("UpdateVehicleIndex", RpcTarget.AllBufferedViaServer, RouteManager.instance._totalActiveCars);
            RouteManager.instance.gameObject.GetComponent<PhotonView>().RPC("NetworkCarTotalUpdate", RpcTarget.AllBufferedViaServer, RouteManager.instance._totalCars + 1);
            float a = Random.value;
            if (a < 0.05f)
            {
                _duplicateCode = "1";
            }

            float b = Random.value;
            if (b < 0.50f)
            {
                _hasDutchLicensePlate = false;
            }
            else
            {
                _hasDutchLicensePlate = true;

                float c = Random.value;
                if (c < 0.30f)
                {
                    _isMilitaryVehicle = true;
                }
            }

            if (_isMilitaryVehicle)
            {
                _middleText = "DM" + _alphabet[Random.Range(0, _alphabet.Length)]; // Add DM into the license plate in case it's a dutch militairy vehicle
            }
            else
            {
                _middleText = _alphabet[Random.Range(0, _alphabet.Length)].ToString() + _alphabet[Random.Range(0, _alphabet.Length)].ToString() + _alphabet[Random.Range(0, _alphabet.Length)].ToString();
            }
            _licensePlate = Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString() + "-" + _middleText + "-" + Random.Range(0, 9).ToString(); // set the license plate

            _photonView.RPC("GenerateLisencePlate", RpcTarget.AllBufferedViaServer, _middleText, _licensePlate, _landCode, _duplicateCode, _hasDutchLicensePlate);
        }
        gameObject.name = RouteManager.instance._totalCars + " Car";
        _photonView.RPC("UpdateAniamtionSpeed", RpcTarget.AllBufferedViaServer, false);
        //_photonView.RPC("UpdateRoute", RpcTarget.AllBufferedViaServer);
        if (PhotonNetwork.IsMasterClient)
        {
            RouteManager.instance.CarQueueUpdate(1);
            RPCActiveCars(true);
            RPCArrivingCars(true);
        }
        _photonView.RPC("UpdateDriveAnimations", RpcTarget.AllBufferedViaServer, _movePos1);
        StartCoroutine(StartSteerCheck());

    }
    [PunRPC]
    private void GenerateLisencePlate(string l_middleText, string l_licensePlate, string l_landCode, string l_duplicateCode, bool l_isDutchPlate)
    {

        if (!l_isDutchPlate)
        {
            l_landCode = _landCodes[Random.Range(1, _landCodes.Length)];
        }
        else
        {
            l_landCode = _landCodes[0];
        }
        _landCode = l_landCode;


        for (int i = 0; i < _licensePlates.Length; i++)
        {
            _licensePlates[i]._licenseText.text = l_licensePlate;
            _licensePlates[i]._landCodeText.text = l_landCode;
            _licensePlates[i]._duplicateText.text = l_duplicateCode;

            if (l_isDutchPlate)
            {
                Material[] materials = _licensePlates[i].GetComponent<MeshRenderer>().materials;
                materials[3] = _licensePlates[i]._yellowPlate;
                materials[4] = _licensePlates[i]._yellowPlate;
                _licensePlates[i].GetComponent<MeshRenderer>().materials = materials;
            }
            else if (!l_isDutchPlate)
            {
                Material[] materials = _licensePlates[i].GetComponent<MeshRenderer>().materials;
                materials[3] = _licensePlates[i]._whitePlate;
                materials[4] = _licensePlates[i]._whitePlate;
                _licensePlates[i].GetComponent<MeshRenderer>().materials = materials;
            }
        }
    }
    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //_photonView.RPC("CheckingSensors", RpcTarget.AllBufferedViaServer);
            //if (!_emergencyBrake)
            //    _photonView.RPC("DriveCar", RpcTarget.AllBufferedViaServer);
            //_photonView.RPC("CarBreaking", RpcTarget.AllBufferedViaServer);

            if (_waitForFrame)
            {
                //_photonView.RPC("CheckWaypointDistance", RpcTarget.AllBufferedViaServer);
                //_photonView.RPC("ApplySteer", RpcTarget.AllBufferedViaServer);
                _photonView.RPC("UpdateSound", RpcTarget.AllBufferedViaServer);
            }

            //if (_isMovingBackwards)
            //    _photonView.RPC("MovingBackwards", RpcTarget.AllBufferedViaServer);
            //_photonView.RPC("UpdateAniamtionSpeed", RpcTarget.AllBufferedViaServer, SensorChecker());
        }
    }
    private bool SensorChecker()
    {
        for (int i = 0; i < _sensorHits.Count; ++i)
        {
            if (_sensorHits[i] == true)
            {
                return true;
            }
        }
        return false;
    }
    [PunRPC]
    private void UpdateSound()
    {
        _carAudioSource.pitch = _baseEnginePitch + (_curSpeed * _enginePitchMultiplier);
        _carAudioSource.pitch = Mathf.Clamp(_carAudioSource.pitch, 0.8f, 3f);
    }
    [PunRPC]
    private void ApplySteer()
    {
        //Generate angle to which way to rotate
        Vector3 l_relativeVector = transform.InverseTransformPoint(_nodes[_currentNode].position);
        //Calulate wheel rotation
        float l_newSteer = (l_relativeVector.x / l_relativeVector.magnitude) * _maxSteerAngle;
        //Set steering angle
        _wheelFL.steerAngle = l_newSteer;
        _wheelFR.steerAngle = l_newSteer;
    }
    [PunRPC]
    private void DriveCar()
    {
        _curSpeed = 2 * Mathf.PI * _wheelFL.radius * _wheelFL.rpm * 60 / 1000;
        if (_curSpeed < _maxSpeed && !_isBraking && !_emergencyBrake && !_movingToQuePoint)
        {
            _wheelFL.motorTorque = _maxMotorTorque;
            _wheelFR.motorTorque = _maxMotorTorque;
        }
        else if (_movingToQuePoint && _curSpeed < _maxSpeed && !_isBraking && !_emergencyBrake)
        {
            _wheelFL.motorTorque = _maxMotorTorque / 2;
            _wheelFR.motorTorque = _maxMotorTorque / 2;
        }
        else
        {
            _wheelFL.motorTorque = 0;
            _wheelFR.motorTorque = 0;
        }
    }
    [PunRPC]
    private void CheckWaypointDistance()
    {

        float l_dist = Vector3.Distance(transform.position, _nodes[_currentNode].position);
        if (l_dist < 1.5f)
        {
            if (_currentNode == _nodes.Count - 1)
            {
                //Debug.Log("Reached the end");
                _carState = CarStates.queuing;
                //If the user is the master client update the route of the car
                if (PhotonNetwork.IsMasterClient)
                    RPCUpdateRoute();
                _movingToQuePoint = false;
                _curSpeed = 0;
                _photonView.RPC("UpdateInQueue", RpcTarget.AllBufferedViaServer, true);
                inQueue = true;
                //if (!inQueue)
                //{
                //Set State
                //Current node reset to 0
                _currentNode = 0;
                _photonView.RPC("UpdateCurrentNode", RpcTarget.AllBufferedViaServer, _currentNode);
                _waitingIndex = RouteManager.instance._queuedCars.Count - 1;
                if (PhotonNetwork.IsMasterClient)
                    RPCQueuedCars(true);
                RPCArrivingCars(false);


                //}

            }
            else
            {
                _currentNode++;
                _photonView.RPC("UpdateCurrentNode", RpcTarget.AllBufferedViaServer, _currentNode);
                inQueue = false;
                _photonView.RPC("UpdateInQueue", RpcTarget.AllBufferedViaServer, false);
            }
        }
        if (_currentNode + 1 >= _nodes.Count - 1 && !_movingToQuePoint)
        {
            float l_finishDist = Vector3.Distance(transform.position, _nodes[_nodes.Count - 1].position);
            //if (l_finishDist <= 10 && _carState != CarStates.queuing)
            //{
            //    Debug.Log("Reached final strecht");
            //    _carState = CarStates.queuing;
            //    _currentNode = 0;
            //    _photonView.RPC("UpdateCurrentNode", RpcTarget.AllBufferedViaServer, _currentNode);
            //    RPCUpdateRoute();
            //}

            if (l_finishDist <= 1)
            {
                _isBraking = true;
                _photonView.RPC("FreezeCarPosition", RpcTarget.AllBufferedViaServer, true);
                _photonView.RPC("UpdateIsBraking", RpcTarget.AllBufferedViaServer, true);
            }
        }
    }
    [PunRPC]
    private void FreezeCarPosition(bool l_freeze)
    {
        if (l_freeze)
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        }
    }
    [PunRPC]
    private void UpdateIsBraking(bool l_value)
    {
        _isBraking = l_value;
    }
    [PunRPC]
    private void UpdateInQueue(bool l_value)
    {
        inQueue = l_value;
    }
    [PunRPC]
    public void UpdateHasBeenCheckedValue(bool l_value)
    {
        _hasBeenChecked = l_value;
    }
    [PunRPC]
    private void CarBreaking()
    {
        if (_isBraking)
        {
            _wheelRL.brakeTorque = _maxBrakeTorque;
            _wheelRR.brakeTorque = _maxBrakeTorque;
        }
        else if (_emergencyBrake)
        {
            _wheelRL.brakeTorque = 9000;
            _wheelRR.brakeTorque = 9000;
        }
        else
        {
            _wheelRL.brakeTorque = 0;
            _wheelRR.brakeTorque = 0;
        }
    }
    [PunRPC]
    private void UpdateCurrentNode(int l_node)
    {
        print(gameObject.name + " | " + l_node);
        _currentNode = l_node;
    }
    [PunRPC]
    private void CheckingSensors()
    {
        RaycastHit l_hit;

        for (int i = 0; i < _sensorObjects.Count; i++)
        {
            Vector3 forward = transform.TransformDirection(_sensorLookObjects[i].transform.position) * _raycastRange;
            //Debug.DrawLine(_sensorObjects[8].transform.position, _sensorLookObjects[i].transform.position * _raycastRange, Color.cyan);
            Debug.DrawRay(_sensorObjects[i].transform.position, forward, Color.cyan);
            if (Physics.Raycast(_sensorObjects[i].transform.position, _sensorLookObjects[i].transform.position, out l_hit, _raycastRange))
            {
                l_hit.transform.TryGetComponent(out CarAI thisCheck);
                if (thisCheck != this)
                {
                    Debug.DrawLine(_sensorObjects[i].transform.position, l_hit.point, Color.red);
                    if (l_hit.transform.TryGetComponent(out PlayerMovement l_player) || l_hit.transform.TryGetComponent(out CarAI l_car) || l_hit.transform.TryGetComponent(out BreakBarrier l_barrier))
                    {
                        _sensorHits[i] = true;
                    }
                    else
                    {
                        _sensorHits[i] = false;
                    }
                }
                else
                {
                    _sensorHits[i] = false;
                }
            }
            else
            {
                _sensorHits[i] = false;

            }
        }
    }
    public void RPCUpdateRoute()
    {
        Debug.Log("Update Route");
        _photonView.RPC("UpdateRoute", RpcTarget.AllBufferedViaServer);
    }
    [PunRPC]
    public void UpdateRoute()
    {
        _photonView.RPC("FreezeCarPosition", RpcTarget.AllBufferedViaServer, false);
        switch (_carState)
        {
            case CarStates.arriving:
                _nodes.Clear();
                if (_arriving)
                {
                    Transform[] l_pathTransformsArriving = RouteManager.instance._arriveRoute.ToArray();
                    for (int i = 0; i < l_pathTransformsArriving.Length; i++)
                    {
                        if (l_pathTransformsArriving[i] != transform)
                        {
                            if (i == l_pathTransformsArriving.Length - 1)
                            {
                                _nodes.Add(RouteManager.instance._queuingPositions[RouteManager.instance._totalActiveCars - 1]);
                            }
                            else
                            {
                                _nodes.Add(l_pathTransformsArriving[i]);
                            }
                        }
                    }
                }
                else
                {
                    // Remove the last element
                    if (_nodes.Count > 0)
                    {
                        _nodes.RemoveAt(_nodes.Count - 1);
                    }

                    _nodes.Add(RouteManager.instance._queuingPositions[RouteManager.instance._totalActiveCars - 1]);
                    _waitingIndex = RouteManager.instance._queuedCars.Count - 1;
                }

                break;
            case CarStates.declined:
                _currentNode = 0;
                //_photonView.RPC("UpdateCurrentNode", RpcTarget.AllBufferedViaServer, _currentNode);
                _nodes.Clear();
                _isBraking = false;
                _photonView.RPC("UpdateIsBraking", RpcTarget.AllBufferedViaServer, false);
                Transform[] l_pathTransformsDeclined = RouteManager.instance._declinedRoute.ToArray();
                for (int i = 0; i < l_pathTransformsDeclined.Length; i++)
                {
                    if (l_pathTransformsDeclined[i] != transform)
                    {
                        _nodes.Add(l_pathTransformsDeclined[i]);
                    }
                }
                break;
            case CarStates.accepted:
                _currentNode = 0;
                _photonView.RPC("UpdateCurrentNode", RpcTarget.AllBufferedViaServer, _currentNode);
                _nodes.Clear();
                _isBraking = false;
                _photonView.RPC("UpdateIsBraking", RpcTarget.AllBufferedViaServer, false);
                Transform[] l_pathTransformsAccepted = RouteManager.instance._acceptedRoute.ToArray();
                for (int i = 0; i < l_pathTransformsAccepted.Length; i++)
                {
                    if (l_pathTransformsAccepted[i] != transform)
                    {
                        _nodes.Add(l_pathTransformsAccepted[i]);
                    }
                }
                break;
            case CarStates.queuing:
                _nodes.Clear();
                int l_thisCarIndex = 0;
                for (int i = 0; i < RouteManager.instance._activeCars.Count; i++)
                {
                    if (RouteManager.instance._activeCars[i] == this)
                    {
                        l_thisCarIndex = i;
                    }
                }

                List<Transform> l_pathTransformsQueuing = new List<Transform>();
                l_pathTransformsQueuing.Add(RouteManager.instance.QueMoveUp(l_thisCarIndex));
                for (int i = 0; i < l_pathTransformsQueuing.Count; i++)
                {
                    if (l_pathTransformsQueuing[i] != transform)
                    {
                        _nodes.Add(l_pathTransformsQueuing[i]);
                    }
                }
                break;
        }
    }
    [PunRPC]
    public void TriggerDeclinedRoute()
    {
        StartCoroutine(DeclineRoute());
    }
    public IEnumerator DeclineRoute()
    {
        _isControlable = false;
        _isMovingBackwards = true;
        if (PhotonNetwork.IsMasterClient)
            RPCQueuedCars(false);
        RPCActiveCars(false);
        if (PhotonNetwork.IsMasterClient)
            _photonView.RPC("UpdateDriveAnimations", RpcTarget.AllBufferedViaServer, _denyBase);
        GetComponentInChildren<CollisionChecker>().enabled = false;
        //_photonView.RPC("TurnOnPauseBarrier", RpcTarget.AllBufferedViaServer);
        GetComponent<Animator>().SetFloat("speedMultiplier", 1f);
        //_photonView.RPC("CloseDoors", RpcTarget.AllBufferedViaServer);
        for (int i = 0; i < GetComponent<CarDoorAnimManager>()._openItemsNamed.Count; i++)
        {
            GetComponent<Animator>().SetTrigger(GetComponent<CarDoorAnimManager>()._openItemsNamed[i] + "Close");
        }


        yield return new WaitForSeconds(5.5f);
        _isMovingBackwards = false;
        _isBraking = false;
        //_photonView.RPC("UpdateIsBraking", RpcTarget.AllBufferedViaServer, false);
        _emergencyBrake = false;
        _carState = CarStates.declined;
        //_photonView.RPC("UpdateRoute", RpcTarget.AllBufferedViaServer);
        yield return new WaitForSeconds(3f);
        //_photonView.RPC("TurnOffPauseBarrier", RpcTarget.AllBufferedViaServer);
        for (int i = 0; i < GetComponent<CarDoorAnimManager>()._openItems.Count; i++)
        {
            GetComponent<Animator>().SetTrigger(GetComponent<CarDoorAnimManager>()._openItems[i]._item.ToString() + "Close");
        }
        GetComponent<CarDoorAnimManager>()._openItems.Clear();
        yield return new WaitForSeconds(1f);
        RouteManager.instance._queuedCars[0]._carState = CarStates.inQueue;
        RPCQueuedCars(true);
        RPCActiveCars(true);
        //FilterQeueu();
    }
    [PunRPC]
    private void MovingBackwards()
    {
        transform.position = Vector3.Lerp(transform.position, -transform.forward * 9.5f, 0.4f * Time.deltaTime);
    }
    [PunRPC]
    public void TriggerAcceptedRoute()
    {
        StartCoroutine(AcceptRoute());
    }
    public IEnumerator AcceptRoute()
    {
        _isControlable = false;
        _barrierManager._barrierAnimator.ResetTrigger("Close");
        _barrierManager._barrierAnimator.SetTrigger("Open");
        //RouteManager.instance._queuedCars[1]._photonView.RPC("DisEngadeFor8Seconds", RpcTarget.AllBufferedViaServer);
        if (PhotonNetwork.IsMasterClient)
            _photonView.RPC("UpdateDriveAnimations", RpcTarget.AllBufferedViaServer, _enterBase);
        GetComponent<Animator>().SetFloat("speedMultiplier", 1f);
        if (PhotonNetwork.IsMasterClient)
            RPCQueuedCars(false);
        RPCActiveCars(false);

        //_photonView.RPC("CloseDoors", RpcTarget.AllBufferedViaServer);
        for (int i = 0; i < GetComponent<CarDoorAnimManager>()._openItemsNamed.Count; i++)
        {
            GetComponent<Animator>().SetTrigger(GetComponent<CarDoorAnimManager>()._openItemsNamed[i] + "Close");
        }
        
        yield return new WaitForSeconds(2f);
        _isBraking = false;
        //_photonView.RPC("UpdateIsBraking", RpcTarget.AllBufferedViaServer, false);
        _emergencyBrake = false;
        _carState = CarStates.accepted;
        // _photonView.RPC("UpdateRoute", RpcTarget.AllBufferedViaServer);
        yield return new WaitForSeconds(5f);
        _barrierManager._barrierAnimator.ResetTrigger("Open");
        _barrierManager._barrierAnimator.SetTrigger("Close");
        if (PhotonNetwork.IsMasterClient)
        {
            RouteManager.instance.CarQueueUpdate(-1);
            photonView.RPC("UpdateVehicleIndex", RpcTarget.AllBufferedViaServer, carIndex - 1);

        }
        yield return new WaitForSeconds(1f);
        //RouteManager.instance._queuedCars[0]._carState = CarStates.inQueue;
        RPCQueuedCars(true);
        RPCActiveCars(true);
        //FilterQeueu();
    }

    [PunRPC]
    private void CloseDoors()
    {
        Interactable[] interactableItems = GetComponentsInChildren<Interactable>();

        for (int i = 0; i < interactableItems.Length; i++)
        {
            if (interactableItems[i].opened)
                interactableItems[i].GetComponent<PhotonView>().RPC("InteractWithObject", RpcTarget.AllBufferedViaServer);
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement l_player))  // or the tag of the object pushing
        {
            // Apply counter force to resist pushing
            Vector3 counterForce = -collision.impulse * 0.5f;  // Adjust multiplier as needed
            GetComponent<Rigidbody>().AddForce(counterForce, ForceMode.Impulse);
        }
    }
    [PunRPC]
    public void DisEngageBreak()
    {
        _isBraking = false;
        _photonView.RPC("UpdateIsBraking", RpcTarget.AllBufferedViaServer, false);
    }
    IEnumerator StartSteerCheck()
    {
        yield return new WaitForSeconds(0.25f);
        _waitForFrame = true;
        yield return null;
    }
    [PunRPC]
    public void SyncControllableVariable()
    {
        _isControlable = true;
    }
    public void RPCArrivingCars(bool addOrRemove)
    {
        _photonView.RPC("SyncArrivingCars", RpcTarget.AllBufferedViaServer, addOrRemove);
    }
    [PunRPC]
    public void SyncArrivingCars(bool addOrRemove)
    {
        if (addOrRemove)
            RouteManager.instance._arrivingCars.Add(this);
        else RouteManager.instance._arrivingCars.Remove(this);
    }
    public void RPCQueuedCars(bool addOrRemove)
    {
        _photonView.RPC("SyncQueuedCars", RpcTarget.AllBufferedViaServer, addOrRemove);
    }
    [PunRPC]
    public void SyncQueuedCars(bool addOrRemove)
    {
        RouteManager.instance.FindAllCarsRPC();
    }
    public void RPCActiveCars(bool addOrRemove)
    {
        _photonView.RPC("SyncActiveCars", RpcTarget.AllBufferedViaServer, addOrRemove);
    }
    [PunRPC]
    public void SyncActiveCars(bool addOrRemove)
    {

        RouteManager.instance.FindAllActiveCarsRPC();
    }
    [PunRPC]
    public void QueueSpeedToZero()
    {
        _curSpeed = 0;
    }
    [PunRPC]
    public void UpdateDriveAnimations(string l_animName)
    {
        GetComponent<Animator>().SetTrigger(l_animName);
    }
    [PunRPC]
    public void UpdateAniamtionSpeed(bool l_pause)
    {
        if (l_pause)
        {
            print("Set speed");
            GetComponent<Animator>().SetFloat("speedMultiplier", 0f);
        }
        else GetComponent<Animator>().SetFloat("speedMultiplier", 1f);
    }
    [PunRPC]
    private void UpdateVehicleIndex(int l_index)
    {
        carIndex = l_index;
    }

    public void LongRangeRaycast()
    {
        GetComponentInChildren<CollisionChecker>().LongCheck();
    }

    public void ShortRangeRaycast()
    {
        GetComponentInChildren<CollisionChecker>().ShortCheck();
    }

    private IEnumerator DisableEnable()
    {
        _collisionChecker.enabled = false;
        yield return new WaitForSeconds(8);
        _collisionChecker.enabled = true;
    }
}
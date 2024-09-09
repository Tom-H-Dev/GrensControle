using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static UnityEngine.GraphicsBuffer;

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

    [Header("Data")]
    public bool _isMilitaryVehicle; // Will add DM in the license palte
    public bool _hasDutchLicensePlate; //WIll make the license plate color yellow
    public string _duplicateCode = null; // The little number on how often the driver has lost their vehicle, purely aesthetical and has no fucntion
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

    [Header("Network")]
    public bool _override = false;
    public bool inQue = false;

    private void Start()
    {
        _barrierManager = FindObjectOfType<BarrierManager>();
        Physics.IgnoreLayerCollision(3, 15);
        GetComponent<Rigidbody>().centerOfMass = _com;
        _nodes = new List<Transform>();
        string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // alphabet....
        string _middleText = null;
        if (PhotonNetwork.IsMasterClient || _override)
        {
            RouteManager.instance.CarQueueUpdate(1);
            RPCActiveCars(true);
            RPCArrivingCars(true);
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

            GetComponent<PhotonView>().RPC("GenerateLisencePlate", RpcTarget.AllBufferedViaServer, _middleText, _licensePlate, _landCode, _duplicateCode, _hasDutchLicensePlate);
        }

        GetComponent<PhotonView>().RPC("UpdateRoute", RpcTarget.AllBufferedViaServer);
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
        CheckingSensors();
        DriveCar();
        ApplySteer();
        CarBreaking();

        if (_waitForFrame)
        {
            CheckWaypointDistance();
        }

        if (_isMovingBackwards)
            MovingBackwards();
    }



    private void ApplySteer()
    {
        Vector3 l_relativeVector = transform.InverseTransformPoint(_nodes[_currentNode].position);
        float l_newSteer = (l_relativeVector.x / l_relativeVector.magnitude) * _maxSteerAngle;
        _wheelFL.steerAngle = l_newSteer;
        _wheelFR.steerAngle = l_newSteer;
    }

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

    private void CheckWaypointDistance()
    {
        float l_dist = Vector3.Distance(transform.position, _nodes[_currentNode].position);
        if (l_dist < 0.5f)
        {
            if (_currentNode == _nodes.Count - 1)
            {
                Debug.Log("Reached the end");
                _movingToQuePoint = false;
                if (!inQue)
                {
                    _waitingIndex = RouteManager.instance._queuedCars.Count - 1;
                    _carState = CarStates.queuing;
                    RPCQueuedCars(true);
                    RPCArrivingCars(false);

                    RPCUpdateRoute();

                }
                inQue = true;
            }
            else
            {
                _currentNode++;
                inQue = false;
            }
        }
        if (_currentNode + 1 >= _nodes.Count - 1 && !_movingToQuePoint)
        {
            float l_finishDist = Vector3.Distance(transform.position, _nodes[_nodes.Count - 1].position);
            if (l_finishDist <= 1)
            {
                _isBraking = true;

            }
        }
        //if (_carState == CarStates.queuing)
        //{
        //    float l_finishDist = Vector3.Distance(transform.position, _nodes[0].position);
        //    if (l_finishDist <= 1)
        //    {
        //        _isBraking = true;
        //    }
        //}
    }

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

    private void CheckingSensors()
    {
        RaycastHit l_hit;
        Vector3 l_sensorStartPos = transform.position;
        l_sensorStartPos += transform.forward * _fronstSensorPosition.z;
        l_sensorStartPos += transform.up * _fronstSensorPosition.y;
        Gizmos.color = Color.yellow;

        //Front Center sensor
        if (Physics.Raycast(l_sensorStartPos, transform.forward, out l_hit, _sensorLength))
        {
            Debug.DrawLine(l_sensorStartPos, l_hit.point, Color.red);
            if (l_hit.transform.TryGetComponent(out PlayerMovement l_player) || l_hit.transform.TryGetComponent(out CarAI l_car))
            {
                _emergencyBrake = true;
            }
            else _emergencyBrake = false;
        }
        else _emergencyBrake = false;

        //Front right middle sensor
        l_sensorStartPos += transform.right * _frontSideSensorPos / 2;
        if (Physics.Raycast(l_sensorStartPos, transform.forward, out l_hit, _sensorLength))
        {
            Debug.DrawLine(l_sensorStartPos, l_hit.point, Color.red);
            if (l_hit.transform.TryGetComponent(out PlayerMovement l_player) || l_hit.transform.TryGetComponent(out CarAI l_car))
            {
                _emergencyBrake = true;
            }
            else _emergencyBrake = false;
        }
        else _emergencyBrake = false;

        //Front right sensor
        l_sensorStartPos += transform.right * _frontSideSensorPos;
        if (Physics.Raycast(l_sensorStartPos, transform.forward, out l_hit, _sensorLength))
        {
            Debug.DrawLine(l_sensorStartPos, l_hit.point, Color.red);
            if (l_hit.transform.TryGetComponent(out PlayerMovement l_player) || l_hit.transform.TryGetComponent(out CarAI l_car))
            {
                _emergencyBrake = true;
            }
            else _emergencyBrake = false;
        }
        else _emergencyBrake = false;

        //Right angled sensor
        if (Physics.Raycast(l_sensorStartPos, Quaternion.AngleAxis(_frontSensorAngle, transform.up) * transform.forward, out l_hit, _sensorLength))
        {
            Debug.DrawLine(l_sensorStartPos, l_hit.point, Color.red);
        }

        //Front Left middle sensor
        l_sensorStartPos -= transform.right * 2 * _frontSideSensorPos / 2;
        if (Physics.Raycast(l_sensorStartPos, transform.forward, out l_hit, _sensorLength))
        {
            Debug.DrawLine(l_sensorStartPos, l_hit.point, Color.red);
            if (l_hit.transform.TryGetComponent(out PlayerMovement l_player) || l_hit.transform.TryGetComponent(out CarAI l_car))
            {
                _emergencyBrake = true;
            }
            else _emergencyBrake = false;
        }
        else _emergencyBrake = false;

        //Front Left sensor
        l_sensorStartPos -= transform.right * 2 * _frontSideSensorPos;
        if (Physics.Raycast(l_sensorStartPos, transform.forward, out l_hit, _sensorLength))
        {
            Debug.DrawLine(l_sensorStartPos, l_hit.point, Color.red);
            if (l_hit.transform.TryGetComponent(out PlayerMovement l_player) || l_hit.transform.TryGetComponent(out CarAI l_car))
            {
                _emergencyBrake = true;
            }
            else _emergencyBrake = false;
        }
        else _emergencyBrake = false;

        //left angles sensor
        if (Physics.Raycast(l_sensorStartPos, Quaternion.AngleAxis(-_frontSensorAngle, transform.up) * transform.forward, out l_hit, _sensorLength))
        {
            Debug.DrawLine(l_sensorStartPos, l_hit.point, Color.red);
        }

    }

    public void RPCUpdateRoute()
    {
        Debug.Log("Update Route");
        GetComponent<PhotonView>().RPC("UpdateRoute", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void UpdateRoute()
    {
        switch (_carState)
        {
            case CarStates.arriving:
                _nodes.Clear();
                //_currentNode = 0;
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
                _nodes.Clear();
                _isBraking = false;
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
                _nodes.Clear();
                _isBraking = false;
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
                //_currentNode = 0;
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
        RPCQueuedCars(false);
        RPCActiveCars(false);
        yield return new WaitForSeconds(5.5f);
        _isMovingBackwards = false;
        _isBraking = false;
        _emergencyBrake = false;
        _carState = CarStates.declined;
        GetComponent<PhotonView>().RPC("UpdateRoute", RpcTarget.AllBufferedViaServer);
        yield return new WaitForSeconds(3f);
        RouteManager.instance.CarQueueUpdate(-1);
        FilterQeueu();
    }

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
        
        RPCQueuedCars(false);
        RPCActiveCars(false);
        yield return new WaitForSeconds(2f);
        Debug.Log("Car accepted");
        _isBraking = false;
        _emergencyBrake = false;
        _carState = CarStates.accepted;
        GetComponent<PhotonView>().RPC("UpdateRoute", RpcTarget.AllBufferedViaServer);
        yield return new WaitForSeconds(5f);
        _barrierManager._barrierAnimator.ResetTrigger("Open");
        _barrierManager._barrierAnimator.SetTrigger("Close");
        RouteManager.instance.CarQueueUpdate(-1);
        FilterQeueu();
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

    private void FilterQeueu()
    {
        for (int i = 0; i < RouteManager.instance._queuedCars.Count; i++)
        {
            RouteManager.instance._queuedCars[i]._carState = CarStates.queuing;
            RouteManager.instance._queuedCars[i]._isBraking = false;
            RouteManager.instance._queuedCars[i].GetComponent<PhotonView>().RPC("DisEngageBreak", RpcTarget.AllBufferedViaServer);
            RouteManager.instance._queuedCars[i].RPCUpdateRoute();
        }
        for (int a = 0; a < RouteManager.instance._arrivingCars.Count; a++)
        {
            RouteManager.instance._arrivingCars[a]._arriving = false;
            if (RouteManager.instance._arrivingCars[a]._currentNode >= RouteManager.instance._arrivingCars[a]._nodes.Count - 1)
            {
                _currentNode = 0;
                _nodes.Clear();
                _nodes.Add(RouteManager.instance._queuingPositions[RouteManager.instance._totalActiveCars - 1]);
            }
            else
            {
                _backupCurrentNode = _currentNode;
                RouteManager.instance._arrivingCars[a].RPCUpdateRoute();
            }


        }
    }

    [PunRPC]
    public void DisEngageBreak()
    {
        _isBraking = false;
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



    public void RPCArrivingCars( bool addOrRemove)
    {
        GetComponent<PhotonView>().RPC("SyncArrivingCars", RpcTarget.AllBufferedViaServer, addOrRemove);
    }
    [PunRPC]
    public void SyncArrivingCars(bool addOrRemove)
    {
        if (addOrRemove)
            RouteManager.instance._arrivingCars.Add(this);
        else RouteManager.instance._arrivingCars.Remove(this);
    }

    public void RPCQueuedCars( bool addOrRemove)
    {
        GetComponent<PhotonView>().RPC("SyncQueuedCars", RpcTarget.AllBufferedViaServer, addOrRemove);
    }
    [PunRPC]
    public void SyncQueuedCars(bool addOrRemove)
    {
        if (addOrRemove)
            RouteManager.instance._queuedCars.Add(this);
        else RouteManager.instance._queuedCars.Remove(this);
    }

    public void RPCActiveCars( bool addOrRemove)
    {
        GetComponent<PhotonView>().RPC("SyncActiveCars", RpcTarget.AllBufferedViaServer, addOrRemove);
    }
    [PunRPC]
    public void SyncActiveCars(bool addOrRemove)
    {
        if (addOrRemove)
            RouteManager.instance._activeCars.Add(this);
        else RouteManager.instance._activeCars.Remove(this);
    }


}

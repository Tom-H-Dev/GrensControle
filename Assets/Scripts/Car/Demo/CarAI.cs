using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CarStates
{
    arriving,
    accepted,
    declined
};

public class CarAI : MonoBehaviour
{
    public CarStates _carState = CarStates.arriving;

    [Header("Paths")]
    [SerializeField] private List<Transform> _nodes;
    private int _currentNode = 0;

    [Header("Wheels")]
    public float _maxSteerAngle = 45f;
    public WheelCollider _wheelFL;
    public WheelCollider _wheelFR;
    public WheelCollider _wheelRL;
    public WheelCollider _wheelRR;

    [Header("Eninge")]
    public float _maxMotorTorque = 80f;
    public float _curSpeed;
    public float _maxSpeed = 100f;
    public bool _isBraking = false;
    public float _maxBrakeTorque = 150f;
    public bool _emergencyBrake;


    [Header("Sensors")]
    public float _sensorLength = 5f;
    public Vector3 _fronstSensorPosition = new Vector3(0, 0.5f, 2);
    public float _frontSideSensorPos = 0.2f;
    public float _frontSensorAngle = 30f;

    private void Start()
    {
        _nodes = new List<Transform>();
        RouteManager.instance.CarQueUpdate();
        UpdateRoute();

    }

    private void FixedUpdate()
    {
        CheckingSensors();
        ApplySteer();
        DriveCar();
        CheckWaypointDistance();
        CarBreaking();
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
        if (_curSpeed < _maxSpeed && !_isBraking && !_emergencyBrake)
        {
            _wheelFL.motorTorque = _maxMotorTorque;
            _wheelFR.motorTorque = _maxMotorTorque;
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
            }
            else
            {
                _currentNode++;
            }
        }
        if (_currentNode + 1 >= _nodes.Count - 1)
        {
            float l_finishDist = Vector3.Distance(transform.position, _nodes[_nodes.Count - 1].position);
            if (l_finishDist <= 15)
            {
                _isBraking = true;
            }
        }
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
        }

        //Front right middle sensor
        l_sensorStartPos += transform.right * _frontSideSensorPos / 2;
        if (Physics.Raycast(l_sensorStartPos, transform.forward, out l_hit, _sensorLength))
        {
            Debug.DrawLine(l_sensorStartPos, l_hit.point, Color.red);
            if (l_hit.transform.TryGetComponent(out PlayerMovement l_player) || l_hit.transform.TryGetComponent(out CarAI l_car))
            {
                _emergencyBrake = true;
            }
        }

        //Front right sensor
        l_sensorStartPos += transform.right * _frontSideSensorPos;
        if (Physics.Raycast(l_sensorStartPos, transform.forward, out l_hit, _sensorLength))
        {
            Debug.DrawLine(l_sensorStartPos, l_hit.point, Color.red);
            if (l_hit.transform.TryGetComponent(out PlayerMovement l_player) || l_hit.transform.TryGetComponent(out CarAI l_car))
            {
                _emergencyBrake = true;
            }
        }

        //Right angled sensor
        if (Physics.Raycast(l_sensorStartPos, Quaternion.AngleAxis(_frontSensorAngle, transform.up) * transform.forward, out l_hit, _sensorLength))
        {
            Debug.DrawLine(l_sensorStartPos, l_hit.point, Color.red);
            if (l_hit.transform.TryGetComponent(out PlayerMovement l_player) || l_hit.transform.TryGetComponent(out CarAI l_car))
            {
                _emergencyBrake = true;
            }
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
        }

        //Front Left sensor
        l_sensorStartPos -= transform.right * 2 * _frontSideSensorPos;
        if (Physics.Raycast(l_sensorStartPos, transform.forward, out l_hit, _sensorLength))
        {
            Debug.DrawLine(l_sensorStartPos, l_hit.point, Color.red);
            if (l_hit.transform.TryGetComponent(out PlayerMovement l_player) || l_hit.transform.TryGetComponent(out CarAI l_car))
            {
                _emergencyBrake = true;
            }
        }

        //left angles sensor
        if (Physics.Raycast(l_sensorStartPos, Quaternion.AngleAxis(-_frontSensorAngle, transform.up) * transform.forward, out l_hit, _sensorLength))
        {
            Debug.DrawLine(l_sensorStartPos, l_hit.point, Color.red);
            if (l_hit.transform.TryGetComponent(out PlayerMovement l_player) || l_hit.transform.TryGetComponent(out CarAI l_car))
            {
                _emergencyBrake = true;
            }
        }

    }

    public void UpdateRoute()
    {
        switch (_carState)
        {
            case CarStates.arriving:
                _currentNode = 0;
                Transform[] l_pathTransformsArriving = RouteManager.instance._arriveRoute.ToArray();
                for (int i = 0; i < l_pathTransformsArriving.Length; i++)
                {
                    if (l_pathTransformsArriving[i] != transform)
                    {
                        if (i == l_pathTransformsArriving.Length - 1)
                        {
                            _nodes.Add(RouteManager.instance._queingPositions[RouteManager.instance._totalActiveCars - 1]);
                        }
                        else
                        {
                            _nodes.Add(l_pathTransformsArriving[i]);
                        }
                    }
                }

                break;
            case CarStates.declined:
                _currentNode = 0;
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
                Transform[] l_pathTransformsAccepted = RouteManager.instance._acceptedRoute.ToArray();
                for (int i = 0; i < l_pathTransformsAccepted.Length; i++)
                {
                    if (l_pathTransformsAccepted[i] != transform)
                    {
                        _nodes.Add(l_pathTransformsAccepted[i]);
                    }
                }
                break;
        }
    }

    public IEnumerator DeclineRoute()
    {
        _curSpeed = 2 * Mathf.PI * _wheelFL.radius * _wheelFL.rpm * 60 / 1000;

        _wheelFL.motorTorque = 0;
        _wheelFR.motorTorque = 0;
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator AcceptRoute()
    {
        yield return new WaitForSeconds(1f);
    }
}

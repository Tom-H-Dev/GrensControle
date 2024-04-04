using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BarrierManager : MonoBehaviour
{
    [SerializeField] Vector3 _checkCubeSize;
    [SerializeField] GameObject _insideLocation;
    [SerializeField] LayerMask _layerMask;
    public CarBehaviour _vehicle;
    [SerializeField] Animator _barrierAnimator;
    [SerializeField] bool _isExit;
    Collider[] _colliders;

    [SerializeField] Transform _stopSpot; // Spot from which the stoplocations will be calculated
    [SerializeField] List<Transform> _stopLocations = new List<Transform>(); // List of possible locations for vehicles to stop behind eachother
    [SerializeField] List<bool> _stopLocationOccupied = new List<bool>(); // Bools telling if a spot in the queue is taken or not
    [SerializeField] float _vehicleWaitDistance; // Distance between the parked vehicles\
    [SerializeField] VehicleManager _vehicleManager;
    public CarBehaviour[] _queue; // current vehicles in the queue

    private void Awake()
    {
        _vehicleManager = FindObjectOfType<VehicleManager>();
        for (int i = 0; i < _vehicleManager._maxVehicles; i++)
        {
            _stopLocationOccupied.Add(false);
            GameObject stopPoint = new GameObject("StopSpot" + (i + 1));
            stopPoint.transform.position = new Vector3(_stopSpot.position.x - _vehicleWaitDistance * i, _stopSpot.position.y, _stopSpot.position.z);
            stopPoint.transform.parent = transform;
            _stopLocations.Add(stopPoint.transform);
            
        }
        _queue = new CarBehaviour[_vehicleManager._maxVehicles];
    }

    void Update()
    {
        _colliders = Physics.OverlapBox(transform.position, new Vector3(_checkCubeSize.x, _checkCubeSize.y, _checkCubeSize.z) / 2, Quaternion.identity, _layerMask);
   

        foreach (Collider collider in _colliders)
        {
            print(collider);
            _vehicle = collider.GetComponent<CarBehaviour>();
        }

        if (_colliders == null)
        {
            _vehicle = null;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(VehicleAcceptedCoroutine());
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(VehicleDeniedCoroutine());
        }
    }

    private void OnDrawGizmos()
    {
        if (_isExit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(_checkCubeSize.x, _checkCubeSize.y, _checkCubeSize.z));
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, new Vector3(_checkCubeSize.x, _checkCubeSize.y, _checkCubeSize.z));
        }
    }

    public IEnumerator VehicleDeniedCoroutine()
    {     
        if (_vehicle != null)
        {
            print("Vehicle denied");
            _vehicle.GetComponent<NavMeshAgent>().angularSpeed = 0;
            for (int i = 0; i < _stopLocations.Count; i++)
            {
                _queue[i].gameObject.GetComponent<NavMeshAgent>().angularSpeed = 0;
                print("e");
                _stopLocations[i].transform.position = new Vector3(_stopLocations[i].position.x - _vehicleWaitDistance * 2, _stopLocations[i].position.y, _stopLocations[i].position.z);
            }
        }
        yield return null;
    }

    public IEnumerator VehicleAcceptedCoroutine()
    {
        if (_vehicle != null)
        {
            _barrierAnimator.ResetTrigger("Close");
            _barrierAnimator.SetTrigger("Open");
            yield return new WaitForSeconds(1);
            _vehicle._currentTarget = _vehicleManager.insideBaseLocation;
            print(_vehicle._currentTarget);
            _vehicleManager._currentVehiclesInt--;

            for (int i = 0; i < _queue.Length; i++)
            {
                if (_queue[i] != null)
                {

                    if (i == 0)
                    {
                        _queue[0] = null;
                    }
                    else if (i > _vehicleManager._currentVehiclesInt-1)
                    {
                        _queue[i] = null;
                    }
                    else
                    {
                        _queue[i - 1] = _queue[i];
                        GetStoppingSpot(_queue[i]);
                    }
                    _stopLocationOccupied[i] = false;
                }
                else
                {
                    _stopLocationOccupied[i] = false;
                }
            }

            while (_vehicle != null)
            {
                yield return null;
            }
            yield return new WaitForSeconds(1);
            _barrierAnimator.ResetTrigger("Open");
            _barrierAnimator.SetTrigger("Close");
            yield return null;
        }
    }

    public void GetStoppingSpot(CarBehaviour car)
    {
        for (int i = 0; i < _vehicleManager._maxVehicles; i++)
        {
            if (_stopLocationOccupied[i] == false)
            {
                car._currentTarget = _stopLocations[i];
                _stopLocationOccupied[i] = true;
                break;
            }
        }
    }
}

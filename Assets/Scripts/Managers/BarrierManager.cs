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
    [SerializeField] List<Transform> _driveAwayLocations = new List<Transform>(); //List of places the car will go trough when denied
    [SerializeField] float _vehicleWaitDistance; // Distance between the parked vehicles\
    [SerializeField] VehicleManager _vehicleManager;
    public List<CarBehaviour> _queue; // current vehicles in the queue

    private void Awake()
    {
        _vehicleManager = FindObjectOfType<VehicleManager>();
        for (int i = 0; i < _vehicleManager._maxVehicles; i++)
        {
            GameObject stopPoint = new GameObject("StopSpot" + (i + 1));
            stopPoint.transform.position = new Vector3(_stopSpot.position.x - _vehicleWaitDistance * i, _stopSpot.position.y, _stopSpot.position.z);
            stopPoint.transform.parent = transform;
            _stopLocations.Add(stopPoint.transform);

        }
    }

    void Update()
    {
        _colliders = Physics.OverlapBox(transform.position, new Vector3(_checkCubeSize.x, _checkCubeSize.y, _checkCubeSize.z) / 2, Quaternion.identity, _layerMask);


        foreach (Collider collider in _colliders)
        {
            print(collider);
            _vehicle = collider.GetComponent<CarBehaviour>();
        }

        print(_colliders);
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
            StartCoroutine(Timer(4));
            foreach (CarBehaviour car in _queue)
            {
                if (car != null)
                    car.GetComponent<NavMeshAgent>().angularSpeed = 0;
            }


            for (int i = 0; i < _stopLocations.Count; i++)
            {
                _stopLocations[i].transform.position = new Vector3(_stopLocations[i].position.x - _vehicleWaitDistance * 2, _stopLocations[i].position.y, _stopLocations[i].position.z);
            }

            for (int i = 0; i < _driveAwayLocations.Count; i++)
            {
                print("next stop");
                if (i > 0)
                {
                    foreach (CarBehaviour car in _queue)
                    {
                        if (car != null)
                        {
                            car.GetComponent<NavMeshAgent>().angularSpeed = car._defaultAngularSpeed;
                        }
                    }
                    _vehicle._currentTarget = _driveAwayLocations[i - 1];
                }
                yield return StartCoroutine(WaitForVehicleToReachTarget());
            }

            for (int j = 0; j < _stopLocations.Count; j++)
            {
                _stopLocations[j].transform.position = new Vector3(_stopLocations[j].position.x + _vehicleWaitDistance * 2, _stopLocations[j].position.y, _stopLocations[j].position.z);
            }
        }

        

        yield return null;
    }

    private IEnumerator Timer(int l_time) 
    {
        yield return new WaitForSeconds(l_time);
        RemoveFirstVehicleFromQueue();
    }

    private IEnumerator WaitForVehicleToReachTarget()
    {
        // Wait until the vehicle is within stopping distance of the target
        while (Vector3.Distance(_vehicle.transform.position, _vehicle._currentTarget.position) > _vehicle._stoppingRadius)
        {
            yield return null;
        }
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

            RemoveFirstVehicleFromQueue();

            //while (_vehicle != null)
            //{
            //    yield return null;
            //}
            yield return new WaitForSeconds(1);
            _barrierAnimator.ResetTrigger("Open");
            _barrierAnimator.SetTrigger("Close");
            yield return null;
        }
    }

    public void AddToQueue(CarBehaviour car)
    {
        _queue.Add(car);
        UpdateStopLocation();
    }

    public void RemoveFirstVehicleFromQueue()
    {
        _queue.RemoveAt(0);
        _vehicleManager._currentVehicles.RemoveAt(0);
        _vehicleManager._currentVehiclesInt--;
        UpdateStopLocation();
    }

    public void UpdateStopLocation()
    {
        for (int i = 0; i < _queue.Count; i++)
        {
            _queue[i]._currentTarget = _stopLocations[i];
        }
    }
}

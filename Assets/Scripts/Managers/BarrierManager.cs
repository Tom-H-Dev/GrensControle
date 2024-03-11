using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrierManager : MonoBehaviour
{
    [SerializeField] Vector3 _checkCubeSize;
    [SerializeField] GameObject _insideLocation;
    [SerializeField] LayerMask _layerMask;
    public CarBehaviour _vehicle;
    [SerializeField] Animator _barrierAnimator;
    [SerializeField] bool _isExit;
    Collider[] _colliders;

    [SerializeField] Transform _stopSpot;
    List<Transform> stopLocations = new List<Transform>();
    [SerializeField] float vehicleWaitDistance;
    VehicleManager _vehicleManager;

    private void Start()
    {
        _vehicleManager = FindObjectOfType<VehicleManager>();
        for (int i = 0; i < _vehicleManager._maxVehicles; i++)
        {
            GameObject stopPoint = new GameObject("StopSpot" + (i + 1));
            stopPoint.transform.position = new Vector3(_stopSpot.position.x - 10 * i, _stopSpot.position.y, _stopSpot.position.z);
            stopPoint.transform.parent = transform;
            stopLocations.Add(stopPoint.transform);
            
        }
    }
    void Update()
    {
        _colliders = Physics.OverlapBox(transform.position, new Vector3(_checkCubeSize.x, _checkCubeSize.y, _checkCubeSize.z) / 2, Quaternion.identity, _layerMask);
   

        foreach (Collider collider in _colliders)
        {
            _vehicle = collider.GetComponent<CarBehaviour>();
        }

        if (_colliders == null)
        {
            _vehicle = null;
        }

        if (Input.GetKeyDown("o"))
        {
            StartCoroutine(VehicleAcceptedCoroutine());
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

        for (int i = 0; i < stopLocations.Count; i++)
        {

        }
    }

    public IEnumerator VehicleDeniedCoroutine()
    {
        if (_vehicle != null)
        {

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
            _vehicle.NextStopPoint(_insideLocation);

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    [SerializeField] Vector3 _checkCubeSize;
    [SerializeField] GameObject _startLocation;
    [SerializeField] LayerMask _layerMask;
    private CarBehaviour _vehicle;
    [SerializeField] Animator _entranceBarrier;
    [SerializeField] Animator _exitBarrier;

    void Update()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(_checkCubeSize.x, _checkCubeSize.y, _checkCubeSize.z) / 2, Quaternion.identity, _layerMask);

       foreach (Collider collider in colliders)
        {
            _vehicle = collider.GetComponent<CarBehaviour>();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(_checkCubeSize.x, _checkCubeSize.y, _checkCubeSize.z));
    }

    public void VehicleDenied()
    {

    }

    public void VehicleAccepted()
    {
        _vehicle.NextStopPoint();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BarrierManager : MonoBehaviour
{
    [SerializeField] Vector3 _checkCubeSize;
    [SerializeField] GameObject _insideLocation;
    [SerializeField] LayerMask _layerMask;
    public CarAI _vehicle;
    public Animator _barrierAnimator;
    [SerializeField] bool _isExit;

    [SerializeField] Transform _stopSpot; // Spot from which the stoplocations will be calculated
    public List<CarBehaviour> _queue; // current vehicles in the queue


    private void Start()
    {
        GetComponent<BoxCollider>().size = _checkCubeSize;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<CarAI>())
        {
            CarAI l_carAI = other.gameObject.GetComponentInParent<CarAI>();
            _vehicle = l_carAI;
            _vehicle._isControlable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponentInParent<CarAI>())
        {
            _vehicle = null;
        }
    }
}

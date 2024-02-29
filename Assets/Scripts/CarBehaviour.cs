using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.AI;
using Unity.VisualScripting;

public class CarBehaviour : MonoBehaviour
{
    NavMeshAgent _agent;
    [SerializeField] GameObject[] _wheels; //LF, RF, LB, RB
    [SerializeField] Transform[] _stopLocations;
    [SerializeField] GameObject _currentTarget;
    [SerializeField] float _normalSpeed;
    [Header("Radius")]
    [SerializeField] float _slowingRadius;
    [SerializeField] float _brakingRadius;
    [SerializeField] float _stoppingRadius;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(_currentTarget.transform.position);
        _agent.stoppingDistance = _stoppingRadius;

        float agentToFinishDistance = Vector3.Distance(transform.position, _currentTarget.transform.position);
        //print(agentToFinishDistance);

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _slowingRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _brakingRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _stoppingRadius);
    }
}

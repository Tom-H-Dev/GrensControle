using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EmergencyBreak : MonoBehaviour
{
    [SerializeField] private CarBehaviour carBehaviour;
    [SerializeField] private Transform _EmergencyBreakPos;
    [SerializeField] LayerMask _playerLayer;

    [SerializeField] private List<Transform> _startTransforms;
    [SerializeField] private List<Transform> _endTransforms;
    [SerializeField] private List<bool> _hitSomething;


    private void Update()
    {
        for (int i = 0; i < _startTransforms.Count; i++)
        {
            Vector3 direction = _endTransforms[i].position - _startTransforms[i].position;
            if (Physics.Raycast(_startTransforms[i].position, direction, out RaycastHit hit, direction.magnitude, _playerLayer))
            {
                Debug.Log("Hit Something: " + hit.transform.name);
                if (hit.transform.gameObject.TryGetComponent(out PlayerMovement player))
                {
                    Debug.Log("Is Player");
                    if (!carBehaviour._emergencyBrake)
                    {
                        _hitSomething[i] = true;
                        Debug.Log("Braking!");
                        carBehaviour._emergencyBrake = true;
                        carBehaviour._agent.speed = 0;
                        carBehaviour._agent.isStopped = true;
                    }
                }
                
            } 
            else
            {
                Debug.Log("Nothing detected");
                _hitSomething[i] = false;
                if (IsAllMissionComplete())
                {
                    carBehaviour._emergencyBrake = false;
                    carBehaviour._agent.isStopped = false;
                }
            }

            Debug.DrawLine(_startTransforms[i].position, _endTransforms[i].position, Color.cyan);
        }
    }
    private bool IsAllMissionComplete()
    {
        // Check if all bool variables in the list are false
        return _hitSomething.All(b => b == false); ;
    }
}

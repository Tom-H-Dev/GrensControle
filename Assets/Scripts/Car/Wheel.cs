using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wheel : MonoBehaviour
{
    public bool _isLeftWheel;
    public float _sped;
    void Update()
    {
        Vector3 rotation = transform.localEulerAngles;
        _sped = GetComponentInParent<NavMeshAgent>().speed;
        if (_isLeftWheel)
        {
            rotation.z += -90 * Time.deltaTime * _sped;
        }
        else
        {
            rotation.z += 90 * Time.deltaTime * _sped;
        }
        transform.localEulerAngles = rotation;
    }
}

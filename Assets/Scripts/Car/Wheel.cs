using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wheel : MonoBehaviour
{
    public bool _isLeftWheel;
    public float _speed;
    void Update()
    {
        Vector3 rotation = transform.localEulerAngles;
        _speed = GetComponentInParent<NavMeshAgent>().speed;
        if (_isLeftWheel)
        {
            rotation.z += -90 * Time.deltaTime * _speed;
        }
        else
        {
            rotation.z += 90 * Time.deltaTime * _speed;
        }
        transform.localEulerAngles = rotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAI : MonoBehaviour
{
    public WheelCollider _targetWheel;

    private Vector3 _wheelPos = new Vector3();
    private Quaternion _wheelRot = new Quaternion();

    private void Update()
    {
        _targetWheel.GetWorldPose(out _wheelPos, out _wheelRot);
        transform.position = _wheelPos;
        transform.rotation = _wheelRot;
    }
}

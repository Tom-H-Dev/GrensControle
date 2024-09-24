using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarRange : MonoBehaviour
{
    public Vector3 Size;
    private void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawWireCube(transform.position, Size);
    }
}

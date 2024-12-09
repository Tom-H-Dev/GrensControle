using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastRangeChanger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CarAI car))
        {
            print("etner");
            car._raycastRange = 30;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CarAI car))
        {
            print("exit");
            car._raycastRange = 4;
        }
    }
}

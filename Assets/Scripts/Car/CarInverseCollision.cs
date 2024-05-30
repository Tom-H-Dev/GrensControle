using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInverseCollision : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement l_player))  // or the tag of the object pushing
        {
            // Apply counter force to resist pushing
            Vector3 counterForce = -collision.impulse * 0.5f;  // Adjust multiplier as needed
            GetComponentInParent<Rigidbody>().AddForce(counterForce, ForceMode.Impulse);
        }
    }
}


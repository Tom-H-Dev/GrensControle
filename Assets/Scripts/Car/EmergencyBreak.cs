using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EmergencyBreak : MonoBehaviour
{
    private CarBehaviour carBehaviour;
    private void Start()
    {
        carBehaviour = GetComponentInParent<CarBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMovement l_player) || other.gameObject.TryGetComponent(out CarBehaviour l_otherCar))
        {
            if (!carBehaviour._emergencyBrake)
            {
                print("Braking!");
                carBehaviour._emergencyBrake = true;
                carBehaviour._agent.speed = 0;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMovement l_player) || other.gameObject.TryGetComponent(out CarBehaviour l_otherCar))
        {
            carBehaviour._emergencyBrake = false;
        }
    }
}

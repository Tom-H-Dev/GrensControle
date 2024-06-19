using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCloseChecker : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMovement l_player))
        {
            GetComponentInParent<Interactable>()._canOpen = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMovement l_player))
        {
            GetComponentInParent<Interactable>()._canOpen = true;
        }
    }
}

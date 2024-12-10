using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCloseChecker : MonoBehaviour
{
    [SerializeField] private Interactable _interactable;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMovement l_player))
        {
            _interactable._canOpen = false;
            print("can't open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMovement l_player))
        {
            _interactable._canOpen = true;
        }
    }
}

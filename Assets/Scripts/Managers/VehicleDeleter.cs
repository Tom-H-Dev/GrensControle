using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDeleter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CarBehaviour l_car))
        {
            PhotonNetwork.Destroy(l_car.gameObject);            
        }
    }
}

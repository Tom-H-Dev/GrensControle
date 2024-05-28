using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDeleter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<CarAI>())
        {
            PhotonNetwork.Destroy(other.gameObject.GetComponentInParent<CarAI>().gameObject);            
        }
    }
}

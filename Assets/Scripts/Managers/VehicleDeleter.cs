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
            PhotonView[] photonViews = other.gameObject.GetComponentsInChildren<PhotonView>();
            GameObject[] _photonItems = new GameObject[photonViews.Length];

            for (int i = 0; i < photonViews.Length; i++)
            {
                _photonItems[i] = photonViews[i].gameObject;
            }
            for (int i = 0; i < _photonItems.Length; i++)
            {
                PhotonNetwork.Destroy(_photonItems[i]);
            }
            PhotonNetwork.Destroy(other.gameObject.GetComponentInParent<CarAI>().gameObject);            
        }
    }
}

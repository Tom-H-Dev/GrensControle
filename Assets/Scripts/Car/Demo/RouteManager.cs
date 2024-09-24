using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public static RouteManager instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("Routes")]
    public List<Transform> _arriveRoute = new List<Transform>();
    public List<Transform> _acceptedRoute = new List<Transform>();
    public List<Transform> _declinedRoute = new List<Transform>();

    [Header("Cars info")]
    public int _totalActiveCars = 0;
    public int _maximumVehicles = 5;
    public List<Transform> _queuingPositions = new List<Transform>();
    public List<CarAI> _activeCars = new List<CarAI>();
    public List<CarAI> _queuedCars = new List<CarAI>();
    public List<CarAI> _arrivingCars = new List<CarAI>();




    public void CarQueueUpdate(int l_index)
    {
        GetComponent<PhotonView>().RPC("NetworkCarQueueUpdate", RpcTarget.AllBufferedViaServer, l_index);
    }

    [PunRPC]
    private void NetworkCarQueueUpdate(int l_index)
    {
        _totalActiveCars += l_index;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _arriveRoute.Count; i++)
        {
            if (i != 0)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_arriveRoute[i - 1].position, _arriveRoute[i].position);
                Gizmos.DrawSphere(_arriveRoute[i].position, 0.3f);
            }
        }
        for (int i = 0; i < _acceptedRoute.Count; i++)
        {
            if (i != 0)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_acceptedRoute[i - 1].position, _acceptedRoute[i].position);
                Gizmos.DrawSphere(_acceptedRoute[i].position, 0.3f);
            }
        }
        for (int i = 0; i < _declinedRoute.Count; i++)
        {
            if (i != 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_declinedRoute[i - 1].position, _declinedRoute[i].position);
                Gizmos.DrawSphere(_declinedRoute[i].position, 0.3f);
            }
        }

        for (int i = 0; i < _queuingPositions.Count; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(_queuingPositions[i].position, 0.3f);
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                _activeCars[0].GetComponent<PhotonView>().RPC("TriggerAcceptedRoute", RpcTarget.AllBufferedViaServer);
                for (int i = 0; i < _activeCars.Count; i++)
                {
                    _activeCars[i]._isBraking = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                _activeCars[0].GetComponent<PhotonView>().RPC("TriggerDeclinedRoute", RpcTarget.AllBufferedViaServer);
                for (int i = 0; i < _activeCars.Count; i++)
                {
                    _activeCars[i]._isBraking = false;
                }
            }
        }
    }

    public Transform QueMoveUp(int l_index)
    {
        return _queuingPositions[l_index];
    }
}

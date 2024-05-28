using Photon.Pun;
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
    public List<Transform> _queingPositions = new List<Transform>();
    public List<CarAI> _activeCars = new List<CarAI>();

    public void CarQueUpdate( int l_index)
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
                Gizmos.DrawLine(_arriveRoute[i-1].position, _arriveRoute[i].position);
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

        for (int i = 0; i < _queingPositions.Count; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(_queingPositions[i].position, 0.3f);
        }
    }

    public void UpdateCarsLocations()
    {
        for (int i = 0; i < _activeCars.Count; i++)
        {
            _activeCars[i]._carState = CarStates.queuing;
            _activeCars[i]._isBraking = false;
            _activeCars[i]._movingToQuePoint = true;
            _activeCars[i].GetComponent<PhotonView>().RPC("UpdateRoute", RpcTarget.AllBufferedViaServer);
        }
    }

    private void Update()
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

    public Transform QueMoveUp(int l_index)
    {
        return _queingPositions[l_index];
    }
}

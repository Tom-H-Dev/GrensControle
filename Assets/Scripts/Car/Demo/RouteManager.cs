using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public static RouteManager instance;
    [SerializeField] private BarrierManager _barrierManager;

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
                    _activeCars[i].GetComponent<PhotonView>().RPC("UpdateIsBraking", RpcTarget.AllBufferedViaServer, false);

                }
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                _activeCars[0].GetComponent<PhotonView>().RPC("TriggerDeclinedRoute", RpcTarget.AllBufferedViaServer);
                for (int i = 0; i < _activeCars.Count; i++)
                {
                    _activeCars[i]._isBraking = false;
                    _activeCars[i].GetComponent<PhotonView>().RPC("UpdateIsBraking", RpcTarget.AllBufferedViaServer, false);
                }
            }
        }
    }

    public Transform QueMoveUp(int l_index)
    {
        return _queuingPositions[l_index];
    }

    public void FindAllCarsRPC()
    {
 
        GetComponent<PhotonView>().RPC("FindAllCars", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void FindAllCars()
    {
        _queuedCars.Clear();

        _queuedCars = GetAllObjectsOnlyInScene();


        SortCarsBySpeed(_queuedCars);

        for (int i = 0; i < _queuedCars.Count; i++)
        {
            if (_queuedCars[i].carIndex == 0 || _queuedCars[i]._carState != CarStates.queuing)
                _queuedCars.RemoveAt(i);
        }
    }

    public void FindAllActiveCarsRPC()
    {

        GetComponent<PhotonView>().RPC("FindAllActiveCars", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void FindAllActiveCars()
    {
        _activeCars.Clear();

        _activeCars = GetAllObjectsOnlyInScene();

        SortCarsBySpeed(_activeCars);
    }

    public void SortCarsBySpeed(List<CarAI> carList)
    {
        carList.Sort((car1, car2) => car1.carIndex.CompareTo(car2.carIndex));
    }

    public static List<CarAI> GetAllObjectsOnlyInScene()
    {
        List<CarAI> objectsInScene = new List<CarAI>();

        foreach (CarAI carAI in Resources.FindObjectsOfTypeAll<CarAI>())
        {
            // Check if the object is in a valid scene (not an asset or prefab)
            if (carAI.gameObject.scene.IsValid() &&
                (carAI.hideFlags != HideFlags.NotEditable && carAI.hideFlags != HideFlags.HideAndDontSave))
            {
                objectsInScene.Add(carAI);
            }
        }

        return objectsInScene;
    }
}

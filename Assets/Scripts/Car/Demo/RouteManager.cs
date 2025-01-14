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
    public int _totalCars;
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

    public void CarTotalUpdate(int total)
    {
        GetComponent<PhotonView>().RPC("NetworkCarTotalUpdate", RpcTarget.AllBufferedViaServer, total);
    }

    [PunRPC]
    private void NetworkCarTotalUpdate(int total)
    {
        _totalCars = total;
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
    }

    public void FindAllActiveCarsRPC()
    {

        GetComponent<PhotonView>().RPC("FindAllActiveCars", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void FindAllActiveCars()
    {
        _activeCars.Clear();

        _activeCars = GetAllObjectsOnlyInScenes();

        SortCarsBySpeed(_activeCars);
    }

    public void SortCarsBySpeed(List<CarAI> carList)
    {
        carList.Sort((car1, car2) => car1.name.CompareTo(car2.name));

    }

    public static List<CarAI> GetAllObjectsOnlyInScene()
    {
        List<CarAI> objectsInScene = new List<CarAI>();

        foreach (CarAI carAI in Resources.FindObjectsOfTypeAll<CarAI>())
        {
            // Check if the object is in a valid scene (not an asset or prefab) and has the required _carState
            if (carAI.gameObject.scene.IsValid() &&
                (carAI.hideFlags != HideFlags.NotEditable && carAI.hideFlags != HideFlags.HideAndDontSave) &&
                carAI._carState == CarStates.queuing)
            {
                objectsInScene.Add(carAI);
            }
        }

        return objectsInScene;
    }

    public static List<CarAI> GetAllObjectsOnlyInScenes()
    {
        List<CarAI> objectsInScene = new List<CarAI>();

        foreach (CarAI carAI in Resources.FindObjectsOfTypeAll<CarAI>())
        {
            // Check if the object is in a valid scene (not an asset or prefab) and has the required _carState
            if (carAI.gameObject.scene.IsValid() &&
                (carAI.hideFlags != HideFlags.NotEditable && carAI.hideFlags != HideFlags.HideAndDontSave))
            {
                objectsInScene.Add(carAI);
            }
        }

        return objectsInScene;
    }
}

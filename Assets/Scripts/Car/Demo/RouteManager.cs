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

    public void CarQueUpdate()
    {
        _totalActiveCars++;
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
}

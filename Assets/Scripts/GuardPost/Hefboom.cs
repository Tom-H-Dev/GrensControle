using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hefboom : MonoBehaviour
{
    [SerializeField] private BarrierManager _entranceBarrierManager;
    [SerializeField] private CorrectCarManager _correctCarManager;
    public bool _isCorrect = true;
    public void OpenGate()
    {
        Debug.Log("Opening Gate");
        //Check if car is in ther area infront of gate
        _entranceBarrierManager.StartCoroutine(_entranceBarrierManager.VehicleAcceptedCoroutine());
        _correctCarManager.ChangeList(_isCorrect, true);
    }

    public void RefuseVehicle()
    {
        Debug.Log("Denied Vehicle");
        //Check if car is in ther area infront of gate
        _entranceBarrierManager.StartCoroutine(_entranceBarrierManager.VehicleDeniedCoroutine());
        _correctCarManager.ChangeList(_isCorrect, false);
    }
}

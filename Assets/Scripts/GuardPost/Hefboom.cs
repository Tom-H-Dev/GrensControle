using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hefboom : MonoBehaviour
{
    [SerializeField] private BarrierManager _entranceBarrierManager;
    public CorrectCarManager _correctCarManager;
    public bool _isCorrect = true;
    public bool _idCorect;
    public bool _driverSus;
    public bool _illigalItems;
    public string _lis;
    //TODO: _illigalItems bool set in car contraband behavior and driver sus asswell
    public void OpenGate()
    {
        Debug.Log("Opening Gate");
        //Check if car is in ther area infront of gate
        _entranceBarrierManager.StartCoroutine(_entranceBarrierManager.VehicleAcceptedCoroutine());
        _correctCarManager._lis = _lis;
        _correctCarManager.ChangeList(_isCorrect, true, _idCorect, _illigalItems, _driverSus);
    }

    public void RefuseVehicle()
    {
        Debug.Log("Denied Vehicle");
        //Check if car is in ther area infront of gate
        _entranceBarrierManager.StartCoroutine(_entranceBarrierManager.VehicleDeniedCoroutine());
        _correctCarManager._lis = _lis;
        _correctCarManager.ChangeList(_isCorrect, false, _idCorect, _illigalItems, _driverSus);
    }
}

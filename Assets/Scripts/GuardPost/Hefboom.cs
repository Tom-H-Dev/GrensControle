using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hefboom : MonoBehaviour
{
    public static Hefboom instance;
    private void Awake()
    {
        instance = this;
    }
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
        if (_entranceBarrierManager._vehicle != null)
        {
            if (!_entranceBarrierManager._vehicle._hasBeenChecked)
            {
                Debug.Log("Opening Gate");
                //Check if car is in ther area infront of gate
                _entranceBarrierManager._vehicle._hasBeenChecked = true;
                StartCoroutine(_entranceBarrierManager._vehicle.AcceptRoute());
                _correctCarManager._lis = _lis;
                if (!_idCorect || _driverSus || _illigalItems)
                    _isCorrect = false;
                else _isCorrect = true;
                _correctCarManager.ChangeList(_isCorrect, true, _idCorect, _illigalItems, _driverSus);
            }
            else
            {
                print("Vehicle allready passed or no vehicles");
            }
        }

        //TODO: if player tries to press button play error sfx from windows
    }

    public void RefuseVehicle()
    {
        if (_entranceBarrierManager._vehicle != null)
        {
            if (!_entranceBarrierManager._vehicle._hasBeenChecked)
            {
                Debug.Log("Denied Vehicle");
                _entranceBarrierManager._vehicle._hasBeenChecked = true;
                //Check if car is in ther area infront of gate
                StartCoroutine(_entranceBarrierManager._vehicle.DeclineRoute());
                _correctCarManager._lis = _lis;
                if (!_idCorect || _driverSus || _illigalItems)
                    _isCorrect = false;
                else _isCorrect = true;
                _correctCarManager.ChangeList(_isCorrect, false, _idCorect, _illigalItems, _driverSus);
            }
            else
            {
                print("Vehicle already declined or no vehicle");
            }
        }
    }
}

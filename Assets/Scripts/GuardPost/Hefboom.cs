using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
    public bool _idCorrect;
    public bool _driverSus;
    public bool _illegalItems;
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
                _entranceBarrierManager._vehicle.GetComponent<PhotonView>().RPC("UpdateHasBeenCheckedValue", RpcTarget.AllBufferedViaServer, true);
                _entranceBarrierManager._vehicle.GetComponent<PhotonView>().RPC("TriggerAcceptedRoute", RpcTarget.AllBufferedViaServer);
                _correctCarManager._lis = _lis;
                if (!_idCorrect || _driverSus || _illegalItems)
                    _isCorrect = false;
                else _isCorrect = true;
                _correctCarManager.ChangeList(_isCorrect, true, _idCorrect, _illegalItems, _driverSus);
            }
            else
            {
                print("Vehicle allready passed or no vehicles");
            }
            GetComponent<PhotonView>().RPC("SetPapers", RpcTarget.AllBufferedViaServer, false);
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
                _entranceBarrierManager._vehicle.GetComponent<PhotonView>().RPC("UpdateHasBeenCheckedValue", RpcTarget.AllBufferedViaServer, true);
                //Check if car is in ther area infront of gate
                _entranceBarrierManager._vehicle.GetComponent<PhotonView>().RPC("TriggerDeclinedRoute", RpcTarget.AllBufferedViaServer);
                ;
                _correctCarManager._lis = _lis;
                if (!_idCorrect || _driverSus || _illegalItems)
                    _isCorrect = false;
                else _isCorrect = true;
                _correctCarManager.ChangeList(_isCorrect, false, _idCorrect, _illegalItems, _driverSus);
            }
            else
            {
                print("Vehicle already declined or no vehicle");
            }
            GetComponent<PhotonView>().RPC("SetPapers", RpcTarget.AllBufferedViaServer, false);
        }
    }
}

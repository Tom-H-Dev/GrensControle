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
    public string _driverName;
    public string _givenDrivername;
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
                if (!_idCorrect && _entranceBarrierManager._vehicle.GetComponent<DriverManager>()._isFalsified)
                {
                    _driverName = _entranceBarrierManager._vehicle.GetComponent<DriverManager>()._driverFirstName +" "+ _entranceBarrierManager._vehicle.GetComponent<DriverManager>()._driverLastName;
                    _givenDrivername = _entranceBarrierManager._vehicle.GetComponent<DriverManager>()._givenFitstName + " " + _entranceBarrierManager._vehicle.GetComponent<DriverManager>()._givenLastName;
                }
                else
                {
                    _driverName = _entranceBarrierManager._vehicle.GetComponent<DriverManager>()._driverFirstName + " " + _entranceBarrierManager._vehicle.GetComponent<DriverManager>()._driverLastName;
                    _givenDrivername = string.Empty;
                }

                if (!_idCorrect || _driverSus || _illegalItems)
                    _isCorrect = false;
                else _isCorrect = true;
                _correctCarManager.ChangeList(_isCorrect, true, _idCorrect, _illegalItems, _driverSus, _driverName, _givenDrivername);
                GetComponent<PhotonView>().RPC("EndDialogue", RpcTarget.AllBufferedViaServer);
            }
            else
            {
                print("Vehicle allready passed or no vehicles");
            }
            GetComponent<PhotonView>().RPC("SetPapers", RpcTarget.AllBufferedViaServer, false);
        }
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
                if (_idCorrect && !_entranceBarrierManager._vehicle.GetComponent<DriverManager>()._isFalsified)
                {
                    _driverName = _entranceBarrierManager._vehicle.GetComponent<DriverManager>()._driverFirstName + " " + _entranceBarrierManager._vehicle.GetComponent<DriverManager>()._driverLastName;
                    _givenDrivername = string.Empty;
                }
                else
                {
                    _driverName = _entranceBarrierManager._vehicle.GetComponent<DriverManager>()._driverFirstName + " " + _entranceBarrierManager._vehicle.GetComponent<DriverManager>()._driverLastName;
                    _givenDrivername = _entranceBarrierManager._vehicle.GetComponent<DriverManager>()._givenFitstName + " " + _entranceBarrierManager._vehicle.GetComponent<DriverManager>()._givenLastName;
                }

                _correctCarManager._lis = _lis;
                if (_idCorrect || !_driverSus || !_illegalItems)
                    _isCorrect = true;
                else _isCorrect = false;
                print(_idCorrect + " " + _driverSus + " " + _illegalItems);
                _correctCarManager.ChangeList(_isCorrect, false, _idCorrect, _illegalItems, _driverSus, _driverName, _givenDrivername);
                GetComponent<PhotonView>().RPC("EndDialogue", RpcTarget.AllBufferedViaServer);
            }
            else
            {
                print("Vehicle already declined or no vehicle");
            }
            GetComponent<PhotonView>().RPC("SetPapers", RpcTarget.AllBufferedViaServer, false);
        }
    }

    [PunRPC]
    public void EndDialogue()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            int team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];

            if (team == 2)
            {
                _entranceBarrierManager._vehicle.GetComponent<carBehaviorDialogue>().dialogue.EndDialogueButton();
            }
        }
    }
}

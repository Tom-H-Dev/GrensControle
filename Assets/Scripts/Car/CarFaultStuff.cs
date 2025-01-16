using Photon.Chat.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CarFaultStuff : ScriptableObject
{
    public string _driversLicense;
    public bool _wasIDWrong;
    public bool _wasDriverSuspicious;
    public bool _wasIllegalItemsInCar;
    public int _vehicleNumber;

    public bool _wasCarAllowed;
    public bool _playerChoice;

    public bool _wasLetThrough;

    public string _driverName;
    public string _givenDrivername;
}
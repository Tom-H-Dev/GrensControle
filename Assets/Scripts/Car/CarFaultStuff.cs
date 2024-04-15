using Photon.Chat.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CarFaultStuff : ScriptableObject
{
    public string _driversLisance;
    public bool _wasIDWrong;
    public bool _wasDriverSus;
    public bool _wasIlligalItemsInCar;
    public int _voertuigNummer;
}
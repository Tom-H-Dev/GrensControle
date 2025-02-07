using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorrectCarManager : MonoBehaviour
{
    public static CorrectCarManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }

    public List<LastFiveList> _lastFive = new List<LastFiveList>();
    public bool demo;
    public bool demo2;
    public string _lis;

    [Header("Last Car")]
    [SerializeField] private TMP_Text _license;
    [SerializeField] private TMP_Text _allowedText, _playerChoiceText;

    [Header("Total Correct")]
    [SerializeField] private TMP_Text _totalVehicleCorrectText;

    [Header("Photon")]
    private PhotonView _photonView;

    [Header("New stuff")]
    public List<CarFaultStuff> _wrongCarsList;
    public int _wrongCarCount = 0;
    public int _totalVehicles;
    public int _playerWrongVehicles;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void UpdateWrongCars(string l_licensePlate, bool l_wasAllowed, bool l_playerChoice, bool l_idWrong, bool l_illigalItemInCar, bool l_driverSus, string l_driverName, string l_giveDriverName)
    {
        _totalVehicles++;
        print("Update cars");
        if (l_wasAllowed != l_playerChoice)
        {
            print("Set wrong updates");
            _playerWrongVehicles++;
            _wrongCarsList[_playerWrongVehicles - 1]._licensePlate = l_licensePlate;
            _wrongCarsList[_playerWrongVehicles - 1]._wasIDWrong = !l_idWrong;
            _wrongCarsList[_playerWrongVehicles - 1]._wasDriverSuspicious = l_driverSus;
            _wrongCarsList[_playerWrongVehicles - 1]._wasIllegalItemsInCar = l_illigalItemInCar;
            _wrongCarsList[_playerWrongVehicles - 1]._vehicleNumber = _totalVehicles;
            _wrongCarsList[_playerWrongVehicles - 1]._wasCarAllowed = l_wasAllowed;
            _wrongCarsList[_playerWrongVehicles - 1]._playerChoice = l_playerChoice;
            _wrongCarsList[_playerWrongVehicles - 1]._wasLetThrough = l_playerChoice;
            _wrongCarsList[_playerWrongVehicles - 1]._driverName = l_driverName;
            _wrongCarsList[_playerWrongVehicles - 1]._givenDrivername = l_giveDriverName;


            if (_playerWrongVehicles >= 3)
            {
                print("Game over");
                SceneManager.LoadScene("AAR");
            }
        }


    }

    public void ChangeList(bool l_wasCorrect, bool l_playerChoice, bool l_idWrong, bool l_illigalItemInCar, bool l_driverSus, string l_driverName, string l_givenDriverName)
    {
        //_photonView.RPC("UpdateList", RpcTarget.AllBufferedViaServer, l_wasCorrect, l_playerChoice);
        _photonView.RPC("UpdateWrongCars", RpcTarget.AllBufferedViaServer, BarrierManager.instance._vehicle._licensePlate,  l_wasCorrect, l_playerChoice, l_idWrong, l_illigalItemInCar, l_driverSus, l_driverName, l_givenDriverName);
    }

    
}

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
    [SerializeField] private TMP_Text _lisence;
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

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (Input.GetKeyDown(KeyCode.Quote))
            {
                ChangeList(demo, demo2, true, true, true);
            }
        }
    }

    [PunRPC]
    public void UpdateWrongCars(bool l_wasAllowed, bool l_playerChoice, bool l_idWrong, bool l_illigalItemInCar, bool l_driverSus)
    {
        _totalVehicles++;

        if (l_wasAllowed != l_playerChoice)
        {
            _playerWrongVehicles++;
            _wrongCarsList[_wrongCarCount - 1]._driversLisance = _lis;
            _wrongCarsList[_wrongCarCount - 1]._wasIDWrong = l_idWrong;
            _wrongCarsList[_wrongCarCount - 1]._wasDriverSus = l_driverSus;
            _wrongCarsList[_wrongCarCount - 1]._wasIlligalItemsInCar = l_illigalItemInCar;
            _wrongCarsList[_wrongCarCount - 1]._voertuigNummer = _totalVehicles;
            _wrongCarsList[_wrongCarCount - 1]._wasCarAllowed = l_wasAllowed;
            _wrongCarsList[_wrongCarCount - 1]._playerChoice = l_playerChoice;
            _wrongCarsList[_wrongCarCount - 1]._wasLetThrough = l_playerChoice;
        }

        if (_playerWrongVehicles >= 3)
        {
            print("Game over");
            //TODO:
            //Scene switch
            SceneManager.LoadScene("AAR");
            //Leave Room
        }
    }

    public void ChangeList(bool l_wasCorrect, bool l_playerChoice, bool l_idWrong, bool l_illigalItemInCar, bool l_driverSus)
    {
        //_photonView.RPC("UpdateList", RpcTarget.AllBufferedViaServer, l_wasCorrect, l_playerChoice);
        _photonView.RPC("UpdateWrongCars", RpcTarget.AllBufferedViaServer, l_wasCorrect, l_playerChoice, l_idWrong, l_illigalItemInCar, l_driverSus);
    }
}

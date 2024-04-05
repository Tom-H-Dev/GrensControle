using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    public int _totalVehicles;
    public int _playerCorrectVehicles;
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
                ChangeList(demo, demo2);
            }
        }
    }

    
    public void ChangeList(bool l_wasCorrect, bool l_playerChoice)
    {
        _photonView.RPC("UpdateList", RpcTarget.AllBufferedViaServer, l_wasCorrect, l_playerChoice);
    }

    [PunRPC]
    private void UpdateList(bool l_wasCorrect, bool l_playerChoice)
    {
        _totalVehicles++;
        if (l_wasCorrect == l_playerChoice)
            _playerCorrectVehicles++;


        for (int i = 4; i >= 0; i--)
        {
            if (i != 0)
            {
                _lastFive[i]._playerChoice = _lastFive[i - 1]._playerChoice;
                _lastFive[i]._isCorrect = _lastFive[i - 1]._isCorrect;

                _lastFive[i]._licensePlateText.text = _lastFive[i - 1]._licensePlateText.text;

                _lastFive[i]._correctText.text = _lastFive[i - 1]._correctText.text;
                _lastFive[i]._correctText.color = _lastFive[i - 1]._correctText.color;

                _lastFive[i]._playerChoiceText.text = _lastFive[i - 1]._playerChoiceText.text;
                _lastFive[i]._playerChoiceText.color = _lastFive[i - 1]._playerChoiceText.color;
            }
        }
        _photonView.RPC("LastCar", RpcTarget.AllBufferedViaServer, l_wasCorrect, l_playerChoice);
        _photonView.RPC("TotalCorrect", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void LastCar(bool l_wasCorrect, bool l_playerChoice)
    {
        _lastFive[0]._playerChoice = l_playerChoice;
        _lastFive[0]._isCorrect = l_wasCorrect;

        _lastFive[0]._licensePlateText.text = _lis;
        if (l_wasCorrect)
        {
            _lastFive[0]._correctText.text = "Ja";
            _lastFive[0]._correctText.color = Color.green;
        }
        else
        {
            _lastFive[0]._correctText.text = "Nee";
            _lastFive[0]._correctText.color = Color.red;
        }

        if (l_playerChoice)
        {
            _lastFive[0]._playerChoiceText.text = "Ja";
            _lastFive[0]._playerChoiceText.color = Color.green;
        }
        else
        {
            _lastFive[0]._playerChoiceText.text = "Nee";
            _lastFive[0]._playerChoiceText.color = Color.red;
        }

        _lisence.text = "Kenteken:\n" + _lastFive[0]._licensePlateText.text;
        if (l_wasCorrect)
        {
            _allowedText.text = "\nJa";
            _allowedText.color = Color.green;
        }
        else
        {
            _allowedText.text = "\nNee";
            _allowedText.color = Color.red;
        }

        if (l_playerChoice)
        {
            _playerChoiceText.text = "\nJa";
            _playerChoiceText.color = Color.green;
        }
        else
        {
            _playerChoiceText.text = "\nNee";
            _playerChoiceText.color = Color.red;
        }
    }
    
    [PunRPC]
    private void TotalCorrect()
    {
        int _totalVehicle = _totalVehicles; // For example
        int _correctVehicles = _playerCorrectVehicles; // For example

        float l_percentageCorrect = (_correctVehicles / (float)_totalVehicle) * 100f;

        _totalVehicleCorrectText.text = _playerCorrectVehicles + " / " + _totalVehicles + " voertuigen correct" + "\n\n" + (int)l_percentageCorrect + "% / 100%\ncorrect";

    }
}

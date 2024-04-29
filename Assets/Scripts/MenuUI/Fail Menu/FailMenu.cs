using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailMenu : MonoBehaviour
{
    public List<FailList> _uiItems;
    public List<CarFaultStuff> _wrongCarData;
    [SerializeField] private TMP_Text _totalVehiclesText;

    void Start()
    {
        SetData();
    }

    private void SetData()
    {
        _totalVehiclesText.text = _totalVehiclesText.text + " " + _wrongCarData[2]._voertuigNummer;

        for (int i = 0; i < _uiItems.Count; i++)
        {
            _uiItems[i]._driversLisanceText.text = "Kenteken:\n" + _wrongCarData[i]._driversLisance;
            if (_wrongCarData[i]._wasIDWrong)
                _uiItems[i]._idCorrectText.text = "Foute identiteit:\nJa";
            else _uiItems[i]._idCorrectText.text = "Foute identiteit:\nNee";

            if (_wrongCarData[i]._wasDriverSus)
                _uiItems[i]._driverText.text = "Verdachte Bestuurder:\nJa";
            else _uiItems[i]._driverText.text = "Verdachte Bestuurder:\nNee";

            if (_wrongCarData[i]._wasIlligalItemsInCar)
                _uiItems[i]._illigalItemsText.text = "Verboden Spullen:\nJa";
            else _uiItems[i]._illigalItemsText.text = "Verboden Spullen:\nNee";

            _uiItems[i]._carNumberText.text = "Voertuig: " + _wrongCarData[i]._voertuigNummer;

            if (_wrongCarData[i]._wasLetThrough)
                _uiItems[i]._wasLetThrough.text = "Is voertuig doorgelaten:\nDoorgelaten";
            else _uiItems[i]._wasLetThrough.text = "Is voertuig doorgelaten:\nAfgewezen";
        }
    }

    public void ToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Main");
    }
}

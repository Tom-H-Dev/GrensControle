using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailMenu : MonoBehaviourPunCallbacks
{
    public List<FailList> _uiItems;
    public List<CarFaultStuff> _wrongCarData;
    [SerializeField] private TMP_Text _totalVehiclesText;
    public PhotonView _photonView;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _photonView = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
            _photonView.RPC("SetData", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void SetData()
    {
        _totalVehiclesText.text = _totalVehiclesText.text + " " + _wrongCarData[2]._vehicleNumber;

        for (int i = 0; i < _uiItems.Count; i++)
        {
            bool named = false;
            _uiItems[i]._afwijkingLijst.text = string.Empty;
            _uiItems[i]._infoLijst.text = string.Empty;
            _uiItems[i]._infoLijst.text += "Kenteken: " + _wrongCarData[i]._licensePlate;
            if (_wrongCarData[i]._wasLetThrough)
                _uiItems[i]._infoLijst.text += "\nVoertuig is doorgelaten";
            else _uiItems[i]._infoLijst.text += "\nVoertuig is afgewezen";

            _uiItems[i]._carIndex.text = "Voertuig Nr: " + _wrongCarData[i]._vehicleNumber;

            if (_wrongCarData[i]._wasIllegalItemsInCar)
            {
                if (_wrongCarData[i]._wasLetThrough)
                {
                    if (_uiItems[i]._afwijkingLijst.text == string.Empty)
                        _uiItems[i]._afwijkingLijst.text += "Er lagen illegale spullen in de auto";
                    else _uiItems[i]._afwijkingLijst.text += "\nEr lagen illegale spullen in de auto";
                }
            }
            else
            {
                if (!_wrongCarData[i]._wasLetThrough)
                    if (_uiItems[i]._afwijkingLijst.text == string.Empty)
                        _uiItems[i]._afwijkingLijst.text += "Er lagen geen illegale spullen in de auto";
                    else _uiItems[i]._afwijkingLijst.text += "\nEr lagen geen illegale spullen in de auto";
            }

            if (_wrongCarData[i]._wasIDWrong)
            {
                if (_wrongCarData[i]._wasLetThrough)
                {
                    _uiItems[i]._afwijkingLijst.text += "\nBestuurder had valse identiteit";
                    if (!named)
                    {
                        named = true;
                        _uiItems[i]._infoLijst.text += $"\nEcht naam van bestuurder was: {_wrongCarData[i]._driverName}";
                        _uiItems[i]._afwijkingLijst.text += $"\nNepnaam bestuurder was: {_wrongCarData[i]._givenDrivername}";
                    }
                }
            }
            else
            {
                if (!_wrongCarData[i]._wasLetThrough)
                    _uiItems[i]._afwijkingLijst.text += "\nBestuurder had geen valse identiteit";
            }


            if (_wrongCarData[i]._wasDriverSuspicious)
            {
                if (_wrongCarData[i]._wasLetThrough)
                {
                    _uiItems[i]._afwijkingLijst.text += "\nBestuurder heeft gelogen";
                    if (!named)
                    {
                        named = true;
                        _uiItems[i]._infoLijst.text += $"\nEcht naam van bestuurder was: {_wrongCarData[i]._driverName}";
                        _uiItems[i]._afwijkingLijst.text += $"\nNepnaam bestuurder was: {_wrongCarData[i]._givenDrivername}";
                    }
                }
            }
            else
            {
                if (!_wrongCarData[i]._wasLetThrough)
                    _uiItems[i]._afwijkingLijst.text += "\nBestuurder heeft niet gelogen";
            }
        }
    }

    public void ToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Main");
    }
}

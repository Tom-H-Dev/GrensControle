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
            _uiItems[i]._infoLijst.text = "Kenteken: " + _wrongCarData[i]._licensePlate;
            if (_wrongCarData[i]._wasIDWrong)
            {
                if (_wrongCarData[i]._wasLetThrough)
                    _uiItems[i]._afwijkingLijst.text += "\nBestuurder had valse identiteit";
                else continue;
            }
            else
            {
                if (!_wrongCarData[i]._wasLetThrough)
                    _uiItems[i]._afwijkingLijst.text += "\nBestuurder had geen valse identiteit";
                else continue;
            }

            
            if (_wrongCarData[i]._wasDriverSuspicious)
            {
                if (_wrongCarData[i]._wasLetThrough)
                    _uiItems[i]._afwijkingLijst.text += "\nBestuurder heeft gelogen";
                else continue;
            }
            else
            {
                if (!_wrongCarData[i]._wasLetThrough)
                    _uiItems[i]._afwijkingLijst.text += "\nBestuurder heeft niet gelogen";
                else continue;
            }

            if (_wrongCarData[i]._wasIllegalItemsInCar)
            {
                if (_wrongCarData[i]._wasLetThrough)
                    _uiItems[i]._afwijkingLijst.text += "\nEr lagen illegale spullen in de auto";
                else continue;
            }
            else
            {
                if (!_wrongCarData[i]._wasLetThrough)
                    _uiItems[i]._afwijkingLijst.text += "\nEr lagen geen illegale spullen in de auto";
                else continue;
            }

            _uiItems[i]._infoLijst.text = "\nVoertuig: " + _wrongCarData[i]._vehicleNumber;

            if (_wrongCarData[i]._wasLetThrough)
                _uiItems[i]._infoLijst.text = "Voertuig is doorgelaten";
            else _uiItems[i]._infoLijst.text = "Voertuig is afgewezen";
        }
    }

    public void ToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Main");
    }
}

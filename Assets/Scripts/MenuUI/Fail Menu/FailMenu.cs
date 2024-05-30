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
            _uiItems[i]._driversLisanceText.text = "Kenteken:\n" + _wrongCarData[i]._driversLicense;
            if (_wrongCarData[i]._wasIDWrong)
                _uiItems[i]._idCorrectText.text = "Foute identiteit:\nJa";
            else _uiItems[i]._idCorrectText.text = "Foute identiteit:\nNee";

            if (_wrongCarData[i]._wasDriverSuspicious)
                _uiItems[i]._driverText.text = "Verdachte Bestuurder:\nJa";
            else _uiItems[i]._driverText.text = "Verdachte Bestuurder:\nNee";

            if (_wrongCarData[i]._wasIllegalItemsInCar)
                _uiItems[i]._illigalItemsText.text = "Verboden Spullen:\nJa";
            else _uiItems[i]._illigalItemsText.text = "Verboden Spullen:\nNee";

            _uiItems[i]._carNumberText.text = "Voertuig: " + _wrongCarData[i]._vehicleNumber;

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

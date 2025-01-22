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
        _totalVehiclesText.text = _totalVehiclesText.text + " " + AARSceneData.instance._data[2]._vehicleNumber;

        for (int i = 0; i < _uiItems.Count; i++)
        {
            bool named = false;
            _uiItems[i]._afwijkingLijst.text = string.Empty;
            _uiItems[i]._infoLijst.text = string.Empty;
            _uiItems[i]._infoLijst.text += "Kenteken: " + AARSceneData.instance._data[i]._licecnsePlate;
            if (AARSceneData.instance._data[i]._wasLetThrough)
                _uiItems[i]._infoLijst.text += "\nVoertuig is doorgelaten";
            else _uiItems[i]._infoLijst.text += "\nVoertuig is afgewezen";

            _uiItems[i]._carIndex.text = "Voertuig Nr: " + AARSceneData.instance._data[i]._vehicleNumber;




            if (AARSceneData.instance._data[i]._wereIlligalItemsInCar)
            {
                if (AARSceneData.instance._data[i]._wasLetThrough)
                {
                    if (_uiItems[i]._afwijkingLijst.text == string.Empty)
                        _uiItems[i]._afwijkingLijst.text += "Er lagen illegale spullen in de auto";
                    else _uiItems[i]._afwijkingLijst.text += "\nEr lagen illegale spullen in de auto";
                }
            }

            if (AARSceneData.instance._data[i]._wasIDWrong)
            {
                if (AARSceneData.instance._data[i]._wasLetThrough)
                {
                    _uiItems[i]._afwijkingLijst.text += "\nBestuurder had valse identiteit";
                    if (!named)
                    {
                        named = true;
                        _uiItems[i]._afwijkingLijst.text += $"\nNepnaam bestuurder was: {AARSceneData.instance._data[i]._givenDriverName}";
                    }
                }
            }


            if (AARSceneData.instance._data[i]._wasDriverSuspisious)
            {
                if (AARSceneData.instance._data[i]._wasLetThrough)
                {
                    _uiItems[i]._afwijkingLijst.text += "\nBestuurder heeft gelogen";
                    if (!named)
                    {
                        named = true;
                        _uiItems[i]._afwijkingLijst.text += $"\nNepnaam bestuurder was: {AARSceneData.instance._data[i]._givenDriverName}";
                    }
                }
            }
            _uiItems[i]._infoLijst.text += $"\nEcht naam van bestuurder was: {AARSceneData.instance._data[i]._driverName}";

            if (!AARSceneData.instance._data[i]._wasLetThrough && !AARSceneData.instance._data[i]._wasIDWrong && !AARSceneData.instance._data[i]._wasDriverSuspisious && !AARSceneData.instance._data[i]._wereIlligalItemsInCar)
            {
                if (_uiItems[i]._afwijkingLijst.text == string.Empty)
                    _uiItems[i]._afwijkingLijst.text += "Er was niks fout met de bestuurder of de auto";
                else _uiItems[i]._afwijkingLijst.text += "\nEr was niks fout met de bestuurder of de auto";
            }
        }
    }

    public void ToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Main");
    }
}

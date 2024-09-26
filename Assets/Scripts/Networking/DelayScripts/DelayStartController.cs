using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DelayStartController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _delayStartButton; //Button used for creating and joining a game.
    [SerializeField] private GameObject _delayCancelButton; //Button used to stop searching for a game to join.
    [SerializeField] private GameObject _loading;
    [SerializeField] private int _roomSize; //Manual set number of players in the room at one time.
    [SerializeField] private TMP_InputField _nameInputField;

    private void Start()
    {
        _delayStartButton.GetComponent<Button>().interactable = false;
    }
    public override void OnConnectedToMaster() //Callback function for when the first connection is established.
    {
        base.OnConnected();
        PhotonNetwork.AutomaticallySyncScene = true; //Makes it so whatever scene the mas client has a connection to.
        _delayStartButton.SetActive(true);
        _loading.SetActive(false);
    }

    public void DelayStart() //Paired to the Delay start button.
    {
        _delayStartButton.SetActive(false);
        _loading.SetActive(false);
        //_delayCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom(); //First tries to join an existing room.
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        CreateRoom(); //If it fails to join a room then it will try to create its own.
    }

    void CreateRoom()
    {
        int _randomRoomNumber = Random.Range(0, 10000); //Creating a random name for the room.
        RoomOptions _roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = _roomSize };
        PhotonNetwork.CreateRoom("Room" + _randomRoomNumber, _roomOptions); //Attempting to create a new room.
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        CreateRoom(); //Retrying to create a new room with a different name.
    }

    public void DelayCancel() //Paired to the cancel button. Used to stop looking for a room to join.
    {
        //_delayCancelButton.SetActive(false);
        _delayStartButton.SetActive(true);
        _loading.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public void ChangeNickname()
    {
        PhotonNetwork.NickName = _nameInputField.text;
        if (_nameInputField.text != "" || _nameInputField.text != null)
            _delayStartButton.GetComponent<Button>().interactable = true;

        if (_nameInputField.text == string.Empty) _delayStartButton.GetComponent<Button>().interactable = false;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _quickStartButton; // Button is used for creating and joining a game.
    [SerializeField] private GameObject _quickCancelButton; // Button is used to stop searching for a game to join.
    [SerializeField] private int _roomSize; //Manual set the number of player in the room at one time.

    public override void OnConnectedToMaster() //Callback function for when the first connection is established.
    {
        PhotonNetwork.AutomaticallySyncScene = true; //Makes it so whatever scene the master client has access to
        _quickStartButton.SetActive(true);
    }

    public void QuickStart() //Paired to the quick start button
    {
        _quickStartButton.SetActive(false);
        _quickCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom(); //First tries to join an existing room
        Debug.Log("QuickStart");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) //Callback function for when the connection could not be established.
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    void CreateRoom() //Trying to create our own room
    {
        Debug.Log("Creating Room Now");
        int _randomRoomNumber = Random.Range(0, 10000); // Creating a random name for the room
        RoomOptions _roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)_roomSize};
        PhotonNetwork.CreateRoom("Room" + _randomRoomNumber, _roomOptions );//Attempting to create a new room.
        Debug.Log(_randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) //Callback function for if we fail to create a room
    {
        Debug.Log("Failed to create room... trying again");
        CreateRoom(); //Retrying to create a new room with a different name.
    }

    public void QuickCancel()
    {
        _quickCancelButton.SetActive(false);
        _quickStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}

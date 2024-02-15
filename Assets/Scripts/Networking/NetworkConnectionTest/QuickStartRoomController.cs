using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField] private int _multiplayerSceneIndex; //Number for the build index to the multiplay scene

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom() //Callback function for when we succesfully create or join a room.
    {
        Debug.Log("Joined Room");
        base.OnJoinedRoom();
        StartGame();
    }

    private void StartGame() //Function for loading into the multiplayer scene.
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(_multiplayerSceneIndex); //Because of AutoSynScene all players who are connected.
        }
    }
}

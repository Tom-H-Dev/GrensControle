using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayRoomController : MonoBehaviourPunCallbacks
{
    //Scene navigation index.
    [SerializeField] private int _waitingRoomSceneIndex;

    public override void OnEnable()
    {
        //Register to photon callback functions.
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        //Unregister to photon callback functions.
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom() //Callback function for when we successfully create or join a room.
    {
        //Called when our player joins a room.
        //Load into waiting room scene.
        base.OnJoinedRoom();
        SceneManager.LoadScene(_waitingRoomSceneIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyStatusManager : MonoBehaviourPunCallbacks
{
    public bool unityDebug = true;
    void Update()
    {
        /*
        if (PhotonNetwork.PlayerList.Length != PhotonNetwork.CurrentRoom.MaxPlayers && !unityDebug)
        {
            LeaveRoom();
        }
        */
    }

    public void LeaveRoom()
    {
        SceneManager.LoadScene("Main");
        PhotonNetwork.LeaveRoom();
    }
}

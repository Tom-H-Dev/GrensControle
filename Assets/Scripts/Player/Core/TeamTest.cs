using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamTest : MonoBehaviour
{
    private void Start()
    {
        if(PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
{
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = 3;
        }
else //If the player has no CustomProperty is makes a new custom property.
        {
            //Makes a new CustomProperty for the player.
            ExitGames.Client.Photon.Hashtable l_playerProps = new ExitGames.Client.Photon.Hashtable
    {
        {"Team", 3}
    };
            //Adds the new CustomProperty to the player.
            PhotonNetwork.LocalPlayer.SetCustomProperties(l_playerProps);
        }
    }
}

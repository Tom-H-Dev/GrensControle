using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class ChoiceButton : MonoBehaviourPunCallbacks
{
    [Header("User Interface")]
    [SerializeField] private Button _choiceButton;
    [SerializeField] private List<Button> _choiceButtons;
    [SerializeField] private TMP_Text _playerName;

    [Header("Bolleans")]
    public bool _isChosen = false;

    private void Start()
    {
        _playerName.text = string.Empty;
    }

    public void ChooseRoll(int l_team)
    {
        if (_isChosen)
            return;
        photonView.RPC("SetRollChoice", RpcTarget.All, PhotonNetwork.NickName, true, true, 1);

        //Check if the player already has a CustonProperty with the key 'Team'.
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = l_team;
        }
        else //If the player has no CustomProperty is makes a new custom property.
        {
            //Makes a new CustomProperty for the player.
            ExitGames.Client.Photon.Hashtable l_playerProps = new ExitGames.Client.Photon.Hashtable
            {
                {"Team", l_team }
            };
            //Adds the new CustomProperty to the player.
            PhotonNetwork.LocalPlayer.SetCustomProperties(l_playerProps);
        }
    }

    public void LeaveRoll()
    {
        if (!_isChosen)
            return;
        photonView.RPC("SetRollChoice", RpcTarget.All, string.Empty, false, false, -1);

        //Removes the CustomProperty from the player.
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = 0;
        }
        else
        {
            ExitGames.Client.Photon.Hashtable l_playerProps = new ExitGames.Client.Photon.Hashtable
            {
                {"Team", 0 }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(l_playerProps);
        }
    }


    [PunRPC]
    private void SetRollChoice(string l_playerName, bool l_isinteractacble, bool l_isChosen, int l_removeOrAdd)
    {
        _playerName.text = l_playerName;
        _isChosen = l_isChosen;
        for (int i = 0; i < _choiceButtons.Count; i++)
        {
            if (!_choiceButtons[i].GetComponentInParent<ChoiceButton>()._isChosen)
                _choiceButtons[i].interactable = true;
            else _choiceButtons[i].interactable = false;
        }

        DelayWatingRoomController.instance._playersReady += l_removeOrAdd;
        DelayWatingRoomController.instance.PlayerCountUpdate();
    }
}

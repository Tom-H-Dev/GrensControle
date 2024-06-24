using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class ChoiceButton : MonoBehaviourPunCallbacks
{
    [Header("User Interface")]
    [SerializeField] private Button _choiceButton;
    [SerializeField] private List<Button> _choiceButtons;
    [SerializeField] private TMP_Text _playerName;

    [SerializeField] private DelayWatingRoomController _roomController;

    [Header("Bolleans")]
    public bool _isChosen = false;

    private void Start()
    {
        _roomController = GetComponentInParent<DelayWatingRoomController>();
        _playerName.text = string.Empty;
        DelayWatingRoomController.instance.PlayerCountUpdate();

        //Check if the player already has a CustonProperty with the key 'Team'.
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = 0;
        }
        else //If the player has no CustomProperty is makes a new custom property.
        {
            //Makes a new CustomProperty for the player.
            ExitGames.Client.Photon.Hashtable l_playerProps = new ExitGames.Client.Photon.Hashtable
            {
                {"Team", 0 }
            };
            //Adds the new CustomProperty to the player.
            PhotonNetwork.LocalPlayer.SetCustomProperties(l_playerProps);
        }
    }

    public void ChooseRoll(int l_team)
    {
        if (_isChosen)
            return;


        photonView.RPC("SetRollChoice", RpcTarget.All, PhotonNetwork.NickName, true, 1);
        photonView.RPC("DisableButton", RpcTarget.All);

        // Disable other buttons locally
        foreach (Button button in _choiceButtons)
        {
            ChoiceButton choiceButtonScript = button.GetComponentInParent<ChoiceButton>();
            if (choiceButtonScript != this && !choiceButtonScript._isChosen)
            {
                button.interactable = false;
            }
        }

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
        photonView.RPC("SetRollChoice", RpcTarget.All, string.Empty, false, -1);
        photonView.RPC("EnableButton", RpcTarget.All);

        // Disable other buttons locally
        foreach (Button button in _choiceButtons)
        {
            ChoiceButton choiceButtonScript = button.GetComponentInParent<ChoiceButton>();
            if (choiceButtonScript != this && choiceButtonScript._isChosen)
            { 
                button.interactable = false;
            }
        }

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

    /// <summary>
    /// Sets the text under the roll button so that the other players can see that this roll is already chosen.
    /// </summary>
    /// <param name="l_playerName"></ The name of the player that needs to be entered in the ui element.>
    /// <param name="l_isChosen"></ Is the roll already chosen or not so can the other players choose this roll asswell>
    /// <param name="l_removeOrAdd"></ Adds or removes a player from the list of ready players in the DelayWaitingRoom>
    [PunRPC]
    private void SetRollChoice(string l_playerName, bool l_isChosen, int l_removeOrAdd)
    {
        _playerName.text = l_playerName;
        _isChosen = l_isChosen;

        // Disable the button if the roll is chosen
        _choiceButton.interactable = !_isChosen;

        DelayWatingRoomController.instance._playersReady += l_removeOrAdd;
        DelayWatingRoomController.instance.PlayerCountUpdate();
    }

    [PunRPC]
    private void DisableButton()
    {
        _choiceButton.interactable = false;
    }

    [PunRPC]
    private void EnableButton()
    {
        _choiceButton.interactable = true;
    }
    private void Update()
    {
        int _playerCount = PhotonNetwork.PlayerList.Length;
        int _roomsize = PhotonNetwork.CurrentRoom.MaxPlayers;

        if (_playerCount >= _roomsize || _roomController._playerNeedOverride)
        {
            for (int i = 0; i < _choiceButtons.Count; i++)
            {
                if (_choiceButtons[i].GetComponentInParent<ChoiceButton>()._isChosen)
                {
                    _choiceButtons[i].interactable = false;
                }
                else if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
                {
                    if ((int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == 1 || (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == 2 || (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == 3)
                    {
                        _choiceButtons[i].interactable = false;
                    }
                    else _choiceButtons[i].interactable = true;
                }
                else _choiceButtons[i].interactable = true;
            }
        }
    }
}

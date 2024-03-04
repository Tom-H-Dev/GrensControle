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

    [Header("Bolleans")]
    public bool _isChosen = false;

    private void Start()
    {
        _playerName.text = string.Empty;
    }

    public void ChooseRoll()
    {
        if (_isChosen)
            return;
        photonView.RPC("SetRollChoice", RpcTarget.All, PhotonNetwork.NickName, true, true, 1);

        
    }

    public void LeaveRoll()
    {
        if (!_isChosen)
            return;
        photonView.RPC("SetRollChoice", RpcTarget.All, string.Empty, false, false, -1);

        
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

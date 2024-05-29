using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReadySystemExplainScene : MonoBehaviourPunCallbacks
{
    private float _loadingTime = 5f;
    private float _curTime = 5f;
    public int _playersReady = 0;

    [Header("User Interface")]
    [SerializeField] private TMP_Text _playerReadyText;
    [SerializeField] private TMP_Text _countdownTimer;
    [SerializeField] private Toggle _readyToggle;
    [SerializeField] private List<GameObject> _explanationTexts;

    [Header("Photon")]
    private PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _curTime = _loadingTime;

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            int l_team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            _explanationTexts[l_team - 1].SetActive(true);
        }
    }


    void Update()
    {
        if (_playersReady >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            _playerReadyText.gameObject.SetActive(false);
            _countdownTimer.gameObject.SetActive(true);
            _photonView.RPC("Countdown", RpcTarget.AllBufferedViaServer);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SceneManager.LoadScene("Grens");
            }
        }
    }

    public void ReadyToggle()
    {
        if (_readyToggle.isOn)
        {
            _photonView.RPC("ChangeReadyPlayer", RpcTarget.AllBufferedViaServer, 1);
        }
        else
        {
            _photonView.RPC("ChangeReadyPlayer", RpcTarget.AllBufferedViaServer, -1);
        }
    }

    [PunRPC]
    private void ChangeReadyPlayer(int l_playerAddSub)
    {
        _playersReady += l_playerAddSub;
        _playerReadyText.text = _playersReady + "/3 klaar met lezen.";
    }

    [PunRPC]
    private void Countdown()
    {
        _curTime -= Time.deltaTime;

        //Format and display countdown timer.
        string tempTimer = string.Format("{0:00}", _curTime);
        _countdownTimer.text = tempTimer;

        if (_curTime <= 0)
        {
            SceneManager.LoadScene("Grens");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _photonView.RPC("ChangeReadyPlayer", RpcTarget.AllBufferedViaServer, -1);
        SceneManager.LoadScene("Main");
    }
}

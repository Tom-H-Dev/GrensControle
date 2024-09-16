using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PhotonView))]
public class DelayWatingRoomController : MonoBehaviourPunCallbacks
{
    public static DelayWatingRoomController instance;

    /*This object must be attached to an object
    / in the waiting room Scene of your project.*/

    //Photon view for sending rpc that updates the timer.
    public PhotonView _photonView;

    //Scene navigation indexes
    [SerializeField] private int SelectionScreen;
    [SerializeField] private int _menuSceneIndex;
    //Number of players in the room out of the total room size.
    private int _playerCount;
    private int _roomsize;
    [SerializeField] private int _minPlayersToStart;

    //Text variables for holding the displays for the countdown timer and player count.
    [SerializeField] private TMP_Text _roomCountDisplay;
    [SerializeField] private TMP_Text _timerToStartDisplay;

    //Bool values for if the timer can count down.
    private bool _readyToCountDown;
    private bool _readyToStart;
    private bool _startingGame;
    //Countdown timer variables.
    private float _timerToStartGame;
    private float _notFullGameTimer;
    private float _fullGameTimer;
    //Countdown timer reset variables.
    [SerializeField] private float _maxWaitTime;
    [SerializeField] private float _maxFullGameTime;

    public int syncVariable;


    public bool _playerNeedOverride = false;
    [Header("Players")]
    public int _playersReady;
    [SerializeField] private List<Button> _rollChoiceButtons;
    [SerializeField] private GameObject _waitingText;

    private void Start()
    {
        //Initialize variables
        _photonView = GetComponent<PhotonView>();
        _fullGameTimer = _maxFullGameTime;
        _notFullGameTimer = _maxWaitTime;
        _timerToStartGame = _maxWaitTime;

        PlayerCountUpdate();
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //So if the master client is making a new level, that everyone joins that specific level.
        instance = this;
    }

    public void PlayerCountUpdate()
    {
        //Updates player count when players join the room.
        //Displays player count.
        //Triggers countdown timer.
        _playerCount = PhotonNetwork.PlayerList.Length;
        _roomsize = PhotonNetwork.CurrentRoom.MaxPlayers;
        _roomCountDisplay.text = _playerCount + "/" + _roomsize + " Spelers";

        if (_playerCount >= _roomsize)
        {
            _waitingText.SetActive(false);
            for (int i = 0; i < _rollChoiceButtons.Count; i++)
            {
                _rollChoiceButtons[i].interactable = true;
            }
        }
        else if (!_playerNeedOverride)
        {
            print("Set Buttons False");
            _waitingText.SetActive(true);
            for (int i = 0; i < _rollChoiceButtons.Count; i++)
            {
                _rollChoiceButtons[i].interactable = false;
            }
        }


        if (_playersReady == _roomsize)
        {
            _readyToStart = true;
        }
        else
        {
            _readyToCountDown = false;
            _readyToStart = false;

        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Called whenever a new player joins the room.
        base.OnPlayerEnteredRoom(newPlayer);
        PlayerCountUpdate();
        //Send master clients countdown timer to all other players in order to sync time.
        _photonView = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
            _photonView.RPC("RPC_SendTimer", RpcTarget.AllBufferedViaServer, _timerToStartGame);
    }

    [PunRPC]
    private void UpdateDebugReadyBoolOverNetwork(bool l_value)
    {
        _playerNeedOverride = l_value;
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        //RPC for syncing the countdown timer to those that join after it has started the countdown.
        _timerToStartGame = timeIn;
        _notFullGameTimer = timeIn;
        if(timeIn < _fullGameTimer)
        {
            _fullGameTimer = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Called whenever a player leaves the room.
        base.OnPlayerLeftRoom(otherPlayer);
        PlayerCountUpdate();
    }

    private void Update()
    {
        WaitingForMorePlayers();
        PlayerCountUpdate();

        if (PhotonNetwork.IsMasterClient)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                StartGame();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                if (_playerNeedOverride)
                    _photonView.RPC("UpdateDebugReadyBoolOverNetwork", RpcTarget.AllBufferedViaServer, false);
                else
                    _photonView.RPC("UpdateDebugReadyBoolOverNetwork", RpcTarget.AllBufferedViaServer, true);
            }
        }
    }

    void WaitingForMorePlayers()
    {
        //If there is only one player in the room the timer will stop and reset.
        if(_playerCount <= 1)
        {
            ResetTimer();
        }
        //When there is enough players in the room the start timer will begin counting down.
        if(_readyToStart)
        {
            _fullGameTimer -= Time.deltaTime;
            _timerToStartGame = _fullGameTimer;
        }
        //Format and display countdown timer.
        string tempTimer = string.Format("{0:00}", _timerToStartGame);
        _timerToStartDisplay.text = tempTimer;
        //If the countdown timer reaches 0 the game will then start.
        if(_timerToStartGame <= 0f)
        {
            if (_startingGame)
               
                return;
            StartGame();
        }
    }

    void ResetTimer()
    {
        //Resets the countdown timer.
        _timerToStartGame = _maxWaitTime;
        _notFullGameTimer = _maxWaitTime;
        _fullGameTimer = _maxFullGameTime;
    }

    public void StartGame()
    {
        //Multiplayer scene is loaded to start the game.
        _startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(SelectionScreen);
    }

    public void DelayCancel()
    {
        //Public function paired to cancel button in the waiting room scene.
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(_menuSceneIndex);
    }
}

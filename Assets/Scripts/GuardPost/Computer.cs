using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Computer : MonoBehaviour
{
    private bool _isOnPC = false;
    private PlayerMovement _playerMovement;
    private PlayerLook _playerLook;
    private Animator _canvasAnimator;

    [SerializeField] private GameObject _computerScreen;
    [SerializeField] private RectTransform _windows;
    [SerializeField] private TMP_Text _clock;
    private Camera mainCamera;
    [SerializeField] private Transform _playerComputerPosition, _cameraComputerPos, _originalCameraPos; 

    private int _realWorldDay = System.DateTime.Now.Day;
    private int _realWorldMonth = System.DateTime.Now.Month;
    private int _realWorldYear = System.DateTime.Now.Year;
    private int _realWorldHour = System.DateTime.Now.Hour;
    private int _realWorldMinute = System.DateTime.Now.Minute;

    [Header("Sounds")]
    [SerializeField] private AudioClip _startPCAudio;
    [SerializeField] private AudioClip _closePCAudio;
    private AudioSource _computerAudio;
    private void Start()
    {
        _computerAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _clock.text = _realWorldHour + ":" + _realWorldMinute + "\n" + _realWorldDay + "-" + _realWorldMonth + "-" + _realWorldYear;
    }

    /// <summary>
    /// If player 1 interacts using the 'E' key in the PlayerLook script the player calls this function to activate the computer.
    /// </summary>
    /// <param name="l_player"></ This referances to the PlayerMovement script from player 1 so he/she cannot move anymore while on the computer>
    /// <param name="l_look"></ This references to the PlayerLook script from player 1 so he/she cannot look around anymore while on the computer>
    /// <param name="l_canvas"></ The animator from the canvas with the interaction text>
    public void OpenPc(PlayerMovement l_player, PlayerLook l_look, Animator l_canvas)
    {
        Debug.Log("Open Computer");
        _isOnPC = true;
        //Player Cannot Move anymore
        _playerMovement = l_player;
        l_player._canMove = false;
        _playerLook = l_look;
        l_look._canLook = false;
        l_look._canInteract = false;

        //Sounds Effects
        _computerAudio.clip = _startPCAudio;
        _computerAudio.Play();

        //Player lerps toward the pc
        mainCamera = _playerLook.GetComponentInChildren<Camera>();
        //l_player.gameObject.transform.position = Vector3.Lerp(l_player.gameObject.transform.position, _playerComputerPosition.position, 1);
        //l_look.gameObject.transform.position = Vector3.Lerp(l_look.gameObject.transform.position, _playerComputerPosition.position, 1);
        //l_look.transform.LookAt(transform);
        //Player sits down animation

        //Screen in Big
        _computerScreen.SetActive(true);

        //Animations
        _canvasAnimator = l_canvas;
        //Mouse gets enabled
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePC()
    {
        Debug.Log("Close Computer");
        _isOnPC = false;
        _computerScreen.SetActive(false);
        _playerMovement.CanMoveChange(true);
        _playerLook.gameObject.transform.position = Vector3.Lerp(_playerLook.gameObject.transform.position, _playerLook._originalLocation, 1);

        _playerLook._canLook = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(ResetComputer());
    }

    private IEnumerator ResetComputer()
    {
        yield return new WaitForSeconds(0.5f);
        _playerLook._canInteract = true;
    }
}

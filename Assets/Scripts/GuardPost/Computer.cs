using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Computer : MonoBehaviour
{
    private Animator _canvasAnimator;

    [Header("User Interface")]
    [Tooltip("The UI GameObject for the computer screen.")]
    [SerializeField] private GameObject _computerScreen;
    [Tooltip("The windows background.")]
    [SerializeField] private RectTransform _windows;
    [Tooltip("The TMPro text component on where to go.")]
    [SerializeField] private TMP_Text _clock;

    [Header("Player Information")]
    private bool _isOnPC = false;
    [Tooltip("Player camera position.")]
    private Camera mainCamera;
    [Tooltip("The positions for the player to be at  when on computer.")]
    [SerializeField] private Transform _playerComputerPosition, _cameraComputerPos, _originalCameraPos;
    private PlayerLook _playerLook;
    private PlayerMovement _playerMovement;

    [Header("Real world time info")]
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
        //Updates the clock on the computer at all times.
        _clock.text = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + "\n" + System.DateTime.Now.Day + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Year;
    }

    /// <summary>
    /// If player 1 interacts using the 'E' key in the PlayerLook script the player calls this function to activate the computer.
    /// </summary>
    /// <param name="l_player"></ This referances to the PlayerMovement script from player 1 so he/she cannot move anymore while on the computer>
    /// <param name="l_look"></ This references to the PlayerLook script from player 1 so he/she cannot look around anymore while on the computer>
    /// <param name="l_canvas"></ The animator from the canvas with the interaction text>
    public void OpenPc(PlayerMovement l_player, PlayerLook l_look, Animator l_canvas)
    {
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

    public void ClosePC()//Closes the computer
    {
        _isOnPC = false;
        _computerScreen.SetActive(false);
        _playerMovement.CanMoveChange(true);
        //_playerLook.gameObject.transform.position = Vector3.Lerp(_playerLook.gameObject.transform.position, _playerLook._originalLocation, 1);

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

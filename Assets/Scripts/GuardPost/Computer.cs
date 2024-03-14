using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    private bool _isOnPC = false;
    private PlayerMovement _playerMovement;
    private PlayerLook _playerLook;
    private Animator _canvasAnimator;

    [SerializeField] private GameObject _computerScreen;
    [SerializeField] private RectTransform _windows;
    private Camera mainCamera;

    private string _timeStamp = System.DateTime.Now.ToString();
    private int _realWorldDay = System.DateTime.Now.Day;
    private int _realWorldMonth = System.DateTime.Now.Month;
    private int _realWorldYear = System.DateTime.Now.Year;

    void Start()
    {
        Application.OpenURL("https://tenor.com/view/cat-berg-cat-orange-cat-swimming-gif-25177582");
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

        //Player lerps toward the pc
        mainCamera = _playerLook.GetComponentInChildren<Camera>();
        //Player sits down animation

        //Screen in Big
        _computerScreen.SetActive(true);

        //Animations
        _canvasAnimator = l_canvas;
        l_canvas.SetTrigger("FadeOutInteractOpen");
        l_canvas.SetTrigger("InteractClose");
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
        _playerLook._canLook = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(ResetComputer());
        _canvasAnimator.SetTrigger("FadeOutInteractClose");
    }

    private IEnumerator ResetComputer()
    {
        yield return new WaitForSeconds(0.5f);
        _playerLook._canInteract = true;
    }
}

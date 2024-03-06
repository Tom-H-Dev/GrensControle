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
    
    void Update()
    {
        if (_isOnPC)
        {
            if (Input.GetKeyDown(KeyCode.E))
                ClosePC();
        }
    }


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

    private void ClosePC()
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

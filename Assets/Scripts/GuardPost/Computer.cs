using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    private bool _isOnPC = false;
    [SerializeField] private PlayerMovement _playerMovement;
    private PlayerLook _playerLook;

    [SerializeField] private GameObject _computerScreen;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && _isOnPC)
            ClosePC();
    }

    public void OpenPc(PlayerMovement l_player, PlayerLook l_look)
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
        //Player sits down animation
        //Screen in Big
        _computerScreen.SetActive(true);
        //Mouse gets enabled

        //Can scroll through pc
    }

    private void ClosePC()
    {
        Debug.Log("Close Computer");
        _isOnPC = false;
        _computerScreen.SetActive(false);
        _playerMovement.CanMoveChange(true);
        _playerLook._canLook = true;
    }

    private IEnumerator ResetComputer()
    {
        yield return new WaitForSeconds(0.5f);
        _playerLook._canInteract = true;
    }
}

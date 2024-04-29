using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartText : MonoBehaviour
{
    public DialogeManager dialoge;

    private void Start()
    {
        dialoge = FindObjectOfType<DialogeManager>();
    }

    public void DialogeStart(PlayerMovement playerMovement, PlayerLook playerLook)
    {
        Cursor.lockState = CursorLockMode.None;
        
        dialoge.TextStart();
    }
}

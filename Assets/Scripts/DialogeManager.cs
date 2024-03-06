using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogeManager : MonoBehaviour
{
    
    private PlayerMovement PlayerMV;
    private PlayerLook PlayerLK;
    

    void Start()
    {
        PlayerMV = GetComponent<PlayerMovement>();
        PlayerLK = GetComponent<PlayerLook>();
       
        PlayerLK.enabled = true;
        PlayerMV.enabled = true;
    }
    
    public void startText()
    {
        Cursor.lockState = CursorLockMode.None;
        PlayerLK.enabled = false;
        PlayerMV.enabled = false;
    }
    

    
}

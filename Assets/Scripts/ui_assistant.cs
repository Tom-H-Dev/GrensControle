using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_assistant : MonoBehaviour
{
    
    private Text messageText;
    
     private void Awake()
    {
        Application.targetFrameRate = 30;
    }

    private void Start()
    {
       
        textWriter.AddWriter_static(messageText, "hello world!", 1f, true);
    }
}

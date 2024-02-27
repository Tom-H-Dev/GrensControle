using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    private bool isOnPC = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
            ClosePC();
    }

    public void OpenPc()
    {
        isOnPC = true;
        //Player Cannot Move anymore
        //Player lerps toward the pc
        //Player sits down animation
        //Screen in Big
        //Mouse gets enabled
        //Can scroll through pc
    }

    private void ClosePC()
    {
        isOnPC = false;
    }
}

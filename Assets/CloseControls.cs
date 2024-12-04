using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CloseControls : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CloseControlsUI();
        }
    }

    public void CloseControlsUI()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}

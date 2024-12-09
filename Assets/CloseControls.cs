using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CloseControls : MonoBehaviour
{
    [SerializeField] private GameObject toDoListObject;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CloseControlsUI();
        }
    }

    public void CloseControlsUI()
    {
        toDoListObject.SetActive(true);
        Destroy(gameObject);
    }
}

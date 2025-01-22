using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleControls : MonoBehaviour
{
    [SerializeField] private GameObject toDoListObject;
    [SerializeField] private GameObject controlsListObject;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            toDoListObject.SetActive(!toDoListObject.activeSelf);
            controlsListObject.SetActive(!controlsListObject.activeSelf);
        }
    }
}

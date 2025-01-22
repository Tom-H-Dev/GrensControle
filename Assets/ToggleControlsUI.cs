using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleControls : MonoBehaviour
{
    [SerializeField] private GameObject toDoListObject;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        toDoListObject.SetActive(!toDoListObject.activeSelf);
        gameObject.SetActive(!gameObject.activeSelf);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseControls : MonoBehaviour
{
    [SerializeField] private GameObject toDoListObject;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleControls();
        }
    }

    public void ToggleControls()
    {
        toDoListObject.SetActive(!toDoListObject.activeSelf);
        gameObject.SetActive(!gameObject.activeSelf);
    }
}

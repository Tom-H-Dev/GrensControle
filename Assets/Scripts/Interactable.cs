using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Animator _Animator;
    public void InteractWithObject()
    {
        print("interacted with " + gameObject.name);
    }
}

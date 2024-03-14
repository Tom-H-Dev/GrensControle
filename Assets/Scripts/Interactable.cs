using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Animator _animator;

    public void InteractWithObject()
    {
        _animator = GetComponent<Animator>();
        print("interacted with " + gameObject.name);
        _animator.SetTrigger("Open");
    }
}

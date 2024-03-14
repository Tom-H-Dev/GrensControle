using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Animator _animator;
    bool opened = false;
    public void InteractWithObject()
    {
        _animator = GetComponent<Animator>();
        print("interacted with " + gameObject.name);
        opened = !opened;

        if (opened)
        {
            _animator.ResetTrigger("Close");
            _animator.SetTrigger("Open");
        }
        else
        {
            _animator.ResetTrigger("Open");
            _animator.SetTrigger("Close");
        }

    }
}

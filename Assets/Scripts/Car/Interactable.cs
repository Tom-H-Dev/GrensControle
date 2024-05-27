using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableItem
{
    Hood,
    FrontLeftDoor,
    FrontRightDoor,
    RearLeftDoor,
    RearRightDoor,
    GloveBox,
    Trunk
};

public class Interactable : MonoBehaviour
{
    private Animator _animator;
    bool opened = false;

    public InteractableItem _item;
    public void InteractWithObject()
    {
        _animator = GetComponentInParent<Animator>();
        opened = !opened;
        _animator.SetBool(_item.ToString(), opened);
        if (opened)
            _animator.SetTrigger(_item.ToString() + "Open");
        else _animator.SetTrigger(_item.ToString() + "Close");
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
    public bool _canOpen = true;

    public InteractableItem _item;

    [PunRPC]
    public void InteractWithObject()
    {
        if (_canOpen == true)
        {
            _animator = GetComponentInParent<Animator>();
            opened = !opened;
            _animator.SetBool(_item.ToString(), opened);
            if (opened)
                _animator.SetTrigger(_item.ToString() + "Open");
            else _animator.SetTrigger(_item.ToString() + "Close");
        }
    }
}

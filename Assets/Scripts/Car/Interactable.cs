using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

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
    public Animator _animator;
    public bool opened = false;
    public bool _canOpen = true;

    public InteractableItem _item;

    private void Start()
    {
        string objectName = gameObject.name;

        if (Enum.TryParse(objectName, out InteractableItem matchedItem))
        {
            _item = matchedItem;
        }

    }

    [PunRPC]
    public void InteractWithObject()
    {
        if (_canOpen == true)
        {
            _animator = GetComponentInParent<Animator>();
            opened = !opened;
            OpenSync(opened);
            if (opened)
                _animator.SetTrigger(_item.ToString() + "Open");
            else _animator.SetTrigger(_item.ToString() + "Close");
        }
    }

    public void OpenSync(bool value)
    {
        GetComponentInParent<PhotonView>().RPC("RPCOpenSync", RpcTarget.AllBufferedViaServer, value);
    }

    [PunRPC]
    private void RPCOpenSync(bool value)
    {
        opened = value;
    }
}

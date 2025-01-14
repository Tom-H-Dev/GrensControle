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
            {
                _animator.SetTrigger(_item.ToString() + "Open");
                GetComponent<PhotonView>().RPC("SyncList", RpcTarget.AllBufferedViaServer, true, (int)_item);
            }
            else
            {
                _animator.SetTrigger(_item.ToString() + "Close");
                GetComponent<PhotonView>().RPC("SyncList", RpcTarget.AllBufferedViaServer, false, (int)_item);
            }
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
    
    [PunRPC]
    private void SyncList(bool add,int index)
    {
        //Get inbdex and compare to the index of the enum
        //Add or remove
        //Convert enum value to string
        InteractableItem l_item = (InteractableItem)index;

        if (add)
        {
            if (!GetComponentInParent<CarDoorAnimManager>()._openItemsNamed.Contains(l_item.ToString()))
                GetComponentInParent<CarDoorAnimManager>()._openItemsNamed.Add(l_item.ToString());
        }
        else
        {
            if (GetComponentInParent<CarDoorAnimManager>()._openItemsNamed.Contains(l_item.ToString()))
                GetComponentInParent<CarDoorAnimManager>()._openItemsNamed.Remove(l_item.ToString());
        }
    }
}

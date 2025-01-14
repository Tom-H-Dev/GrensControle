using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDoorAnimManager : MonoBehaviour
{
    public List<Interactable> _openItems = new List<Interactable>();
    public List<string> _openItemsNamed = new List<string>();

    public void CloseAnims()
    {
        GetComponent<PhotonView>().RPC("CloseAllOpenItems", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void CloseAllOpenItems()
    {
        for (int i = 0; i < _openItems.Count; i++)
        {
            GetComponent<Animator>().SetTrigger(_openItems[i]._item.ToString() + "Close");
        }
        _openItems.Clear();
    }
}

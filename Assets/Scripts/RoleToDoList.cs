using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleToDoList : MonoBehaviour
{
    [SerializeField] private List<GameObject> RoleLists = new List<GameObject>();
    public int team;
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            RoleLists[team - 1].SetActive(true);
        }
    }
}
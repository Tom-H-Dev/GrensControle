using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMirror : MonoBehaviour
{
    private bool _player3 = false;
    [SerializeField] private GameObject _mirrorCart;
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            int l_team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            if (l_team == 3)
                _player3 = true;
            else _player3 = false;
        }
        _mirrorCart.SetActive(false);
    }

    void Update()
    {
        if (_player3 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_mirrorCart.activeSelf)
                _mirrorCart.SetActive(false);
            else _mirrorCart.SetActive(true);
        }
    }
}

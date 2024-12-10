using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class FPSCounter : MonoBehaviour
{
    public float _fps;
    public float _ping;
    public TMP_Text _fpsText;
    public TMP_Text _pingText;

    private void Start()
    {
        InvokeRepeating(nameof(GetFPS), 1, 0.5f);
        InvokeRepeating(nameof(GetPing), 1, 0.5f);
    }

    public void GetFPS()
    {
        _fps = (int)(1f / Time.unscaledDeltaTime);
        _fpsText.text = "FPS:  " + _fps;
    }

    public void GetPing()
    {
        _ping = PhotonNetwork.GetPing();
        _pingText.text = "Ping:  " + _ping + "ms";
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.PUN;

public class VoiceChatManagerGrens : MonoBehaviour
{
    public static VoiceChatManagerGrens instance;
    private void Awake()
    {
        instance = this;
    }

    public PunVoiceClient _client;
    
    void Start()
    {
        _client = GetComponent<PunVoiceClient>();
    }
}

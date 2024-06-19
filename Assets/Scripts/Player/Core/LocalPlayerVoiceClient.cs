using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.PUN;

public class LocalPlayerVoiceClient : MonoBehaviour
{
    void Start()
    {
        VoiceChatManagerGrens.instance._client.SpeakerPrefab = gameObject;
    }
}

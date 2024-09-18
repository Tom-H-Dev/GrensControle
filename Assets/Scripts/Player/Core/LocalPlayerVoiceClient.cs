using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.PUN;
using Photon.Voice.Unity;

public class LocalPlayerVoiceClient : MonoBehaviour
{
    void Start()
    {
        VoiceChatManagerGrens.instance._client.SpeakerPrefab = gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if(VoiceChatManagerGrens.instance.GetComponent<Recorder>().TransmitEnabled == true)
                VoiceChatManagerGrens.instance.GetComponent<Recorder>().TransmitEnabled = false;
            else VoiceChatManagerGrens.instance.GetComponent<Recorder>().TransmitEnabled = true;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.PUN;
using Photon.Voice.Unity;

public class LocalPlayerVoiceClient : MonoBehaviour
{
    [SerializeField] private GameObject _muteIndication;
    void Start()
    {
        VoiceChatManagerGrens.instance._client.SpeakerPrefab = gameObject;
        if (VoiceChatManagerGrens.instance.GetComponent<Recorder>().TransmitEnabled == true)
            _muteIndication.SetActive(true);
        else _muteIndication.SetActive(false);    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (VoiceChatManagerGrens.instance.GetComponent<Recorder>().TransmitEnabled == true)
            {
                VoiceChatManagerGrens.instance.GetComponent<Recorder>().TransmitEnabled = false;
                _muteIndication.SetActive(true);
            }
            else
            {
                VoiceChatManagerGrens.instance.GetComponent<Recorder>().TransmitEnabled = true;
                _muteIndication.SetActive(false);
            }
        }
    }
}

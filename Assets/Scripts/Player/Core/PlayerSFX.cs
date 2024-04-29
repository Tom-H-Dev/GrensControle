using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private AudioSource _playerAudio;
    [SerializeField] private AudioClip _footstep1;
    [SerializeField] private AudioClip _footstep2;

    public void PlayFirstFootstep()
    {
        _playerAudio.clip = _footstep1;
        _playerAudio.Play();
    }
    public void PlaySecondFootstep()
    {
        _playerAudio.clip = _footstep2;
        _playerAudio.Play();
    }
}

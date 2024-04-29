using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlagboomSFX : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    public void StartSFX()
    {
        _audioSource.Play();
    }
}

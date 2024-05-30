using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneFade : MonoBehaviour
{
    public Animator _animator;
    public int _levelToLoad;

    public void FadeToNextLevel(int _levelIndex)
    {
        _levelToLoad = _levelIndex;
        _animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (PhotonNetwork.IsMasterClient)
                SceneManager.LoadScene(_levelToLoad);
        }
        else SceneManager.LoadScene(_levelToLoad);
    }
}

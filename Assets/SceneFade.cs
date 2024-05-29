using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        //SceneManager.LoadScene(_levelToLoad);
    }
}

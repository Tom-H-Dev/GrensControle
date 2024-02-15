using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviourPunCallbacks
{
    [SerializeField] private int _multiplayerSceneIndex;

    public void ButtonStart()
    {
        SceneManager.LoadScene(_multiplayerSceneIndex);
    }
}

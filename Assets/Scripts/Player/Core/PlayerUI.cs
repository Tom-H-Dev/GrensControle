using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviourPun
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _crossHair;
    public bool _isDoingSomething = false;

    [SerializeField] private GameObject _debugMenu;

    void Update()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !_isDoingSomething)
            {
                PauseMenu();
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                if (_debugMenu.activeSelf)
                    _debugMenu.SetActive(false);
                else _debugMenu.SetActive(true);
            }
        }
    }

    public void PauseMenu()
    {
        if (_pauseMenu.activeSelf)
        {
            //Turn escape menu off.
            GetComponent<PlayerMovement>()._canMove = true;
            GetComponent<PlayerLook>()._canLook = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _crossHair.SetActive(true);

            _pauseMenu.SetActive(false);
        }
        else
        {
            //Turn escape menu on.
            GetComponent<PlayerMovement>()._canMove = false;
            GetComponent<PlayerLook>()._canLook = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _crossHair.SetActive(false);

            _pauseMenu.SetActive(true);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void returnToGame()
    {
        GetComponent<PlayerMovement>()._canMove = true;
        GetComponent<PlayerLook>()._canLook = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _pauseMenu.SetActive(false);
        print("test");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}

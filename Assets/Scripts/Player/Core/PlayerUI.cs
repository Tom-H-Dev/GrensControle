using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviourPun
{
    [SerializeField] private GameObject _pauseMenu;
    
    void Update()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_pauseMenu.active)
                {
                    GetComponent<PlayerMovement>()._canMove = true;
                    GetComponent<PlayerLook>()._canLook = true;

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = false;

                    _pauseMenu.SetActive(false);

                    print("esxc");
                }
                else 
                {
                    GetComponent<PlayerMovement>()._canMove = false;
                    GetComponent<PlayerLook>()._canLook = false;

                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = true;

                    _pauseMenu.SetActive(true); 
                    
                    print("test");
                }
            }
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
}

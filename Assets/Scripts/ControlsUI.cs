using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameControls;
    [SerializeField] private GameObject _mirrorControls;
    bool _gameControlsClosed = false;
    bool _mirrorControlsClosed = false;

    public void CloseControls()
    {
        Destroy(_gameControls);
    }

    public void CloseMirrorControls()
    {
        Destroy(_mirrorControls);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F5))
        {
            CloseControls();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CloseMirrorControls();
        }

        if (_gameControlsClosed && _mirrorControlsClosed)
        {
            Destroy(gameObject);
        }
    }
}

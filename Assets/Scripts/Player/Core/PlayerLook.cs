using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private Vector2 Senstivity;

    private Vector2 XYRotation;

    private void Update()
    {
        PlayerLookAround();
        PlayerLookRaycast();
    }

    private void PlayerLookAround()
    {
        //Get MouseInput of player on axis
        //Up, Down, Left, Right
        Vector2 MouseInput = new Vector2
        {
            x = Input.GetAxis("Mouse X"),
            y = Input.GetAxis("Mouse Y")
        };

        XYRotation.x -= MouseInput.y * Senstivity.y;
        XYRotation.y += MouseInput.x * Senstivity.x;

        //Clamp X mouseLook axis to 75 and -75 to ensure no infinite rotation
        XYRotation.x = Mathf.Clamp(XYRotation.x, -75f, 75f);

        //Handle side to side Input and Left to Right Input
        transform.eulerAngles = new Vector3(0f, XYRotation.y, 0f);
        PlayerCamera.localEulerAngles = new Vector3(XYRotation.x, 0f, 0f);

        //Lock Cursor to playWindow
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void PlayerLookRaycast()
    {
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.forward,out RaycastHit l_hit, 2f))
        {
            if (l_hit.transform.gameObject.TryGetComponent(out Computer l_pc))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    l_pc.OpenPc();
                }
            }
        }
    }
}

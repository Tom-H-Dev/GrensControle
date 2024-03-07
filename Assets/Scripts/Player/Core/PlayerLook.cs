using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private Vector2 Senstivity;

    private Vector2 XYRotation;
    public bool _canLook = true;
    public bool _canInteract = true;
    private Animator _canvasAnimator;

    [SerializeField] private float Reach;

    public int team;
    private void Start()
    {
        _canvasAnimator = GameManager.instance._canvasAnimator;
    }
    private void Update()
    {
        if (_canLook)
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
    bool once = true;
    private void PlayerLookRaycast()
    {
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.forward, out RaycastHit l_hit, Reach))
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
            {
                team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];

                if (team == 1)
                {
                    
                    if (l_hit.transform.gameObject.TryGetComponent(out Computer l_pc) && _canInteract)
                    {
                        if (once)
                        {
                            _canvasAnimator.SetTrigger("InteractOpen");
                            once = false;
                        }

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            l_pc.OpenPc(GetComponent<PlayerMovement>(), this, _canvasAnimator);
                            once = true;
                        }
                    }
                }
                else if(team == 2)
                {
                    if (l_hit.transform.gameObject.TryGetComponent(out DialogeManager l_Text))
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            l_Text.startText();
                        }
                    }
                }
                else
                {
                    Debug.Log("Local Player is not in Team: " + 1);
                }
            }
            
            
            if (l_hit.transform.gameObject.TryGetComponent(out DialogeManager L_Text))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    L_Text.startText();
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        
        Gizmos.DrawRay(transform.position, transform.forward * Reach);
        
        Gizmos.DrawWireSphere(transform.position + transform.forward * Reach, 0.2f);
    }
}

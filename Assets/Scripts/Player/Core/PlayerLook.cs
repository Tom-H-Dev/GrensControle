using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Vector3 _originalLocation;

    public int team;

    [SerializeField] private TMP_Text _interactionText;

    private void Start()
    {
        _canvasAnimator = GameManager.instance._canvasAnimator;
        _originalLocation = transform.position;
        
        //Lock Cursor to playWindow
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (_canLook)
        {
            PlayerLookAround();
            InteractionText();
        }

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
        
    }
    bool once = true;
    private void PlayerLookRaycast()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
                            l_pc.OpenPc(GetComponent<PlayerMovement>(), this, _canvasAnimator);
                            _interactionText.text = "";
                        }
                    }
                    else if (team == 2)
                    {
                        if (l_hit.transform.gameObject.TryGetComponent(out carBehaviorDialogue l_Text))
                        {
                            l_Text.DialogeStart(GetComponent<PlayerMovement>(), this);
                        }
                    }
                    else if (team == 3)
                    {
                        if (l_hit.collider.gameObject.TryGetComponent(out Interactable l_interactable))
                        {
                            l_interactable.InteractWithObject();
                        }
                    }
                    else Debug.LogError("No Team was found");
                }
            }
            if (l_hit.transform.gameObject.TryGetComponent(out carBehaviorDialogue b_Text))
            {
                b_Text.DialogeStart(GetComponent<PlayerMovement>(), this);
            }
        }
    }

    private void InteractionText()
    {
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.forward, out RaycastHit l_hit, Reach))
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
            {
                team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];

                if (team == 1)
                {
                    if (l_hit.transform.gameObject.TryGetComponent(out Computer l_pc) && _canInteract)
                        _interactionText.text = "Druk op 'E' om de computer te openen.";
                    else if (l_hit.transform.gameObject.TryGetComponent(out DialogeManager l_papaers))
                        _interactionText.text = "Druk op 'E' om de papieren op te vragen.";
                    else _interactionText.text = "";
                }
                else if (team == 2)
                {
                    if (l_hit.transform.gameObject.TryGetComponent(out DialogeManager l_Text))
                        _interactionText.text = "Druk op 'E' om met de bestuurder te praten.";
                    else _interactionText.text = "";
                }
                else if (team == 3)
                {
                    if (l_hit.collider.gameObject.TryGetComponent(out Interactable l_interactable))
                        _interactionText.text = "Druk op 'E' om het open te maken.";
                    else _interactionText.text = "";
                }
                else Debug.LogError("No Team was found");
            }
        }
        else _interactionText.text = "";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawRay(transform.position, transform.forward * Reach);

        Gizmos.DrawWireSphere(transform.position + transform.forward * Reach, 0.2f);
    }
}

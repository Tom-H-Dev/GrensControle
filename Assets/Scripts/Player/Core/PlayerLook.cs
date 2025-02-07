using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private Vector2 Sensitivity;

    private Vector2 XYRotation;
    public bool _canLook = true;
    public bool _canInteract = true;
    private Animator _canvasAnimator;

    [SerializeField] private float Reach;
    public Vector3 _originalLocation;

    public int team;

    [SerializeField] private TMP_Text _interactionText;
    public bool _isTalking;
    [SerializeField] private LayerMask _layerMask;

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

        XYRotation.x -= MouseInput.y * Sensitivity.y;
        XYRotation.y += MouseInput.x * Sensitivity.x;

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
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.forward, out RaycastHit l_hit, Reach, _layerMask))
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
                        if (l_hit.transform.gameObject.TryGetComponent(out carBehaviorDialogue l_Text) && l_hit.transform.gameObject.GetComponent<CarAI>()._isControlable && !l_hit.transform.GetComponent<CarAI>()._hasBeenChecked)
                        {
                            l_Text.DialogeStart(GetComponent<PlayerMovement>(), this);
                            _interactionText.text = "";
                        }
                    }
                    else if (team == 2)
                    {
                        if (l_hit.transform.gameObject.TryGetComponent(out carBehaviorDialogue l_Text) && l_hit.transform.gameObject.GetComponent<CarAI>()._isControlable && !l_hit.transform.GetComponent<CarAI>()._hasBeenChecked)
                        {
                            l_Text.DialogeStart(GetComponent<PlayerMovement>(), this);
                            _interactionText.text = "";
                        }
                    }
                    else if (team == 3)
                    {
                        if (l_hit.collider.gameObject.TryGetComponent(out Interactable l_interactable) && l_hit.transform.gameObject.GetComponent<CarAI>()._isControlable && !l_hit.transform.GetComponent<CarAI>()._hasBeenChecked)
                        {
                            l_interactable.GetComponent<PhotonView>().RPC("InteractWithObject", RpcTarget.AllBufferedViaServer);
                            //l_interactable.InteractWithObject();
                            _interactionText.text = "";
                        }
                    }
                    else Debug.LogError("No Team was found");
                }
            }
        }
    }

    private void InteractionText()
    {
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.forward, out RaycastHit l_hit, Reach, _layerMask))
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
            {
                team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];

                if (team == 1)
                {
                    if (l_hit.transform.gameObject.TryGetComponent(out Computer l_pc) && _canInteract)
                        _interactionText.text = "Druk op 'E' om de computer te openen.";
                    else if (l_hit.transform.gameObject.TryGetComponent(out carBehaviorDialogue l_papaers) && !_isTalking && l_hit.transform.gameObject.GetComponent<CarAI>()._isControlable)
                        _interactionText.text = "Druk op 'E' om de papieren op te vragen.";
                    else _interactionText.text = "";
                }
                else if (team == 2)
                {
                    if (l_hit.transform.gameObject.TryGetComponent(out carBehaviorDialogue l_Text) && !_isTalking && l_hit.transform.gameObject.GetComponent<CarAI>()._isControlable)
                        _interactionText.text = "Druk op 'E' om met de bestuurder te praten.";
                    else _interactionText.text = "";
                }
                else if (team == 3)
                {
                    if (l_hit.collider.gameObject.TryGetComponent(out Interactable l_interactable) && l_hit.transform.gameObject.GetComponent<CarAI>()._isControlable)
                        if (l_interactable.opened == true)
                        {
                            _interactionText.text = "Druk op 'E' om dicht te maken.";
                        }
                        else
                        {
                            _interactionText.text = "Druk op 'E' om open te maken.";
                        }

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

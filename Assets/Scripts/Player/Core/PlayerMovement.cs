using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] private float _moveSmoothTime;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;

    private CharacterController _controller;
    private Vector3 _currentMoveVelocity;
    private Vector3 _moveDampVelocity;

    private Vector3 _currentForceVelocity;


    [SerializeField] private Rigidbody _rigidbody;


    private Animator _animator;

    public bool _canMove = true;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            // Get the value of the "Team" property
            int team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
        }
    }

    private void FixedUpdate()
    {

        if (photonView.IsMine)
        {
            if (_canMove)
            {


                Vector3 PlayerInput = new Vector3
                {
                    x = Input.GetAxisRaw("Horizontal") * _walkSpeed,
                    y = 0f,
                    z = Input.GetAxisRaw("Vertical") * _walkSpeed,
                };


                //Get KeyInput of player on axis
                //Forward, Backwards, Left, Right
                //All input cases get constant speed
                if (PlayerInput.magnitude > 1f)
                {
                    PlayerInput.Normalize();
                }

                //Moving playerObject in direction relative to Player lookat
                Vector3 MoveVector = transform.TransformDirection(PlayerInput);
                //LeftShift key sprint
                float CurrentSpeed = IsWalking();

                //smoothing of CurrentMoveVelocity
                _currentMoveVelocity = Vector3.SmoothDamp(_currentMoveVelocity, MoveVector * CurrentSpeed, ref _moveDampVelocity, _moveSmoothTime);

                //Check if character is grounded
                if (_controller.isGrounded == false)
                {
                    //Add our gravity Vector
                    _currentMoveVelocity += Physics.gravity;
                }
                
                _controller.Move(_currentMoveVelocity * Time.deltaTime);
                if (PlayerInput == Vector3.zero)
                    Idle();
            }
        }
    }

    private float IsRunning()
    {
        _animator.SetBool("isRunning", true);
        _animator.SetBool("isWalking", false);
        return _runSpeed;
    }
    private float IsWalking()
    {
        _animator.SetBool("isRunning", false);
        _animator.SetBool("isWalking", true);
        return _walkSpeed;
    }

    private void Idle()
    {
        _animator.SetBool("isRunning", false);
        _animator.SetBool("isWalking", false);
    }

    public void CanMoveChange(bool l_value)
    {
        _canMove = l_value;
    }
}

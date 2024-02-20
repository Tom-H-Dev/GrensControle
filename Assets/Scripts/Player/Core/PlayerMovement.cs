using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] private float MoveSmoothTime;
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float RunSpeed;

    private CharacterController controller;
    private Vector3 CurrentMoveVelocity;
    private Vector3 MoveDampVelocity;

    private Vector3 CurrentForceVelocity;


    [SerializeField] private Rigidbody _rigidbody;
    private float _lastSynchronizationTime = 0f;
    private float _syncDelay = 0f;
    private float _syncTime = 0f;
    private Vector3 _syncStartPosition = Vector3.zero;
    private Vector3 _syncEndPosition = Vector3.zero;

    //void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{

    //    if (stream.IsWriting == true)
    //    {
    //        stream.SendNext(_syncEndPosition);
    //       // stream.SendNext(_syncStartPosition);

    //    }
    //    else if (stream.IsReading)
    //    {
    //        _syncEndPosition = (Vector3)stream.ReceiveNext();
    //        _syncStartPosition = _rigidbody.position;

    //        _syncTime = 0f;
    //        _syncDelay = Time.time - _lastSynchronizationTime; // delay
    //        _lastSynchronizationTime = Time.time;   


    //    }
    //} 

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {

        if (photonView.IsMine)
        {
            Vector3 PlayerInput = new Vector3
            {
                x = Input.GetAxisRaw("Horizontal") * WalkSpeed,
                y = 0f,
                z = Input.GetAxisRaw("Vertical") * WalkSpeed,
            };
            Debug.Log(PlayerInput.x + " | " + PlayerInput.z);
            //_rigidbody.velocity = new Vector3(PlayerInput.x, PlayerInput.y, PlayerInput.z);


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
            float CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;

            //smoothing of CurrentMoveVelocity
            CurrentMoveVelocity = Vector3.SmoothDamp(CurrentMoveVelocity, MoveVector * CurrentSpeed, ref MoveDampVelocity, MoveSmoothTime);

            controller.Move(CurrentMoveVelocity * Time.deltaTime);
        }
        //else
        //{
        //    SyncedMovement();
        //} 
    }

    //private void SyncedMovement()
    //{
    //    _syncTime += Time.deltaTime;
    //    _rigidbody.position = Vector3.Lerp(_syncStartPosition, _syncEndPosition, _syncTime / _syncDelay);
    //} 
}

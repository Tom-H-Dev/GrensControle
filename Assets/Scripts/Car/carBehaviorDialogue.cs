using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class carBehaviorDialogue : MonoBehaviour
{
    public DialogueManager dialogue;
    
    public float madnessTimer = 100f;

    public float Range;

    public int happiness;
    
    public string driverTag = "Driver";
    
    public List<GameObject> cars = new List<GameObject>();
    public float timer;

    private void Start()
    {
        dialogue = FindObjectOfType<DialogueManager>();

        if (PhotonNetwork.IsMasterClient)
        {
            GetComponent<PhotonView>().RPC("RandomHappieness", RpcTarget.AllBufferedViaServer);    
        }
        
    }
    [PunRPC]
    private void RandomHappieness()
    {
        happiness = Random.Range(80, 30);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            cars.Clear();
            inCollider();
            timer = 2f;
        }
        
        if (cars.Count > 1 && cars.IndexOf(this.gameObject) > 0)
        {
            if (madnessTimer >= 0)
            {
                madnessTimer -= Time.deltaTime;
            }
        }
    }

    public void DialogeStart(PlayerMovement playerMovement, PlayerLook playerLook)
    {
        dialogue.TextStart(playerMovement, playerLook);
    }
    public void inCollider()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,Range);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(driverTag))
            {
                cars.Add(collider.gameObject);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}


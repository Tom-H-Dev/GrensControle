using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StartDialoge : MonoBehaviour
{
    private int layermask = 1 << 8;

    private Camera mycam;

    [SerializeField] private float reach;

    [SerializeField] private GameObject talk;

    private PlayerMovement PlayerMV;
    private PlayerLook PlayerLK;
    

    void Start()
    {
        PlayerMV = GetComponent<PlayerMovement>();
        PlayerLK = GetComponent<PlayerLook>();
        mycam = Camera.main;
        talk.SetActive(false);
        
        PlayerLK.enabled = true;
        PlayerMV.enabled = true;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = mycam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit ,reach))
            {
                if (hit.collider.CompareTag("Driver"))
                {
                    startText();
                    Cursor.lockState = CursorLockMode.None;
                    PlayerLK.enabled = false;
                    PlayerMV.enabled = false;
                    this.enabled = false;
                }
            }
        }
    }

    void startText()
    {
        talk.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        
        Gizmos.DrawRay(transform.position, transform.forward * reach);
        
        Gizmos.DrawWireSphere(transform.position + transform.forward * reach, 0.2f);
    }

    public void Active()
    {
        talk.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        PlayerLK.enabled = true;
        PlayerMV.enabled = true;
        this.enabled = true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialoge : MonoBehaviour
{
    private int layermask = 1 << 8;

    private Camera mycam;

    [SerializeField] private float reach;

    [SerializeField] private GameObject talk;

    void Start()
    {
        mycam = Camera.main;
        talk.SetActive(false);
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carBehaviorDialogue : MonoBehaviour
{
    public DialogeManager dialoge;
    
    public float madnessTimer = 100f;

    public float Range;
    
    public string driverTag = "Driver";
    
    public List<GameObject> cars = new List<GameObject>();
    public float timer;

    private void Start()
    {
        dialoge = FindObjectOfType<DialogeManager>();
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
        dialoge.TextStart(playerMovement, playerLook);
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


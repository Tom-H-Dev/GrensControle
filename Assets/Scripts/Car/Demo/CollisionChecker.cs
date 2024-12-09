using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField] private Collider _shortCollider;
    [SerializeField] private Collider _longCollider;

    private void Start()
    {
        ShortCheck();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerMovement l_player))
        {
            print("Collision");
            GetComponentInParent<Animator>().SetFloat("speedMultiplier", 0f);
        }

        if (other.gameObject.layer == 3)
        {
            print("Collision");
            if (other.transform.TryGetComponent(out CarBody l_carBody))
            {
                print("carbody");
                l_carBody.UpdateAnimationSpeed(0f);
            }
            else if (other.transform.GetComponent<CarAI>() || other.transform.GetComponentInParent<CarAI>())
            {
                print("other car");
                GetComponentInParent<Animator>().SetFloat("speedMultiplier", 0f);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerMovement l_player))
        {
            print("Exit");
            GetComponentInParent<Animator>().SetFloat("speedMultiplier", 1f);
        }

        if (other.transform.TryGetComponent(out CarBody l_car))
        {
            print("Exit");
            l_car.UpdateAnimationSpeed(1f);
        }
    }

    public void LongCheck()
    {
        _shortCollider.enabled = false;
        _longCollider.enabled = true;
    }
    public void ShortCheck()
    {
        _longCollider.enabled = false;
        _shortCollider.enabled = true;
    }
}

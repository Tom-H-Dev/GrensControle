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

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerMovement l_player))
        {
            GetComponentInParent<Animator>().SetFloat("speedMultiplier", 0f);
        }

        if (other.gameObject.layer == 3)
        {
            if (other.transform.TryGetComponent(out CarBody l_carBody))
            {
                GetComponentInParent<Animator>().SetFloat("speedMultiplier", 0f);
            }
            else if (other.transform.GetComponent<CarAI>() || other.transform.GetComponentInParent<CarAI>())
            {
                GetComponentInParent<Animator>().SetFloat("speedMultiplier", 0f);
            }
        }
        
        if (other.transform.TryGetComponent(out BreakBarrier l_barrier))
        {
            GetComponentInParent<Animator>().SetFloat("speedMultiplier", 0f);
            GetComponentInParent<CarAI>().ShortRangeRaycast();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerMovement l_player))
        {
            GetComponentInParent<Animator>().SetFloat("speedMultiplier", 1f);
        }

        if (other.gameObject.layer == 3)
        {
            if (other.transform.TryGetComponent(out CarBody l_carBody))
            {
                l_carBody.UpdateAnimationSpeed(1f);
            }
            else if (other.transform.GetComponent<CarAI>() || other.transform.GetComponentInParent<CarAI>())
            {
                GetComponentInParent<Animator>().SetFloat("speedMultiplier", 1f);
            }
        }
        if (other.transform.TryGetComponent(out BreakBarrier l_barrier))
        {
            GetComponentInParent<Animator>().SetFloat("speedMultiplier", 1f);
        }
    }
    private void OnDisable()
    {
            GetComponentInParent<Animator>().SetFloat("speedMultiplier", 0f);
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

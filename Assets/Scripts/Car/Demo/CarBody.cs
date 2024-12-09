using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBody : MonoBehaviour
{
    public void UpdateAnimationSpeed(float l_speed)
    {
        GetComponentInParent<Animator>().SetFloat("speedMultiplier", l_speed);
    }
}

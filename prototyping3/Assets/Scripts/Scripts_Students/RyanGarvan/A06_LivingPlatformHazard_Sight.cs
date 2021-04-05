using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A06_LivingPlatformHazard_Sight : MonoBehaviour
{
    A06_LivingPlatformHazard_Mind our_mind;
    
    void Start()
    {
        our_mind = GetComponentInParent<A06_LivingPlatformHazard_Mind>();
    }

    void OnTriggerEnter(Collider the_interloper)
    {
        our_mind.OnInterloperEnter(the_interloper.gameObject);
    }

    void OnTriggerExit(Collider the_interloper)
    {
        our_mind.OnInterloperExit(the_interloper.gameObject);
    }
}
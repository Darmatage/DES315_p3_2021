using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A06_LivingPlatformHazard_Body : MonoBehaviour
{
    A06_LivingPlatformHazard_Mind our_mind;
    BoxCollider our_physicality;
    bool our_satisfaction = false;

    void Start()
    {
        our_mind = GetComponentInParent<A06_LivingPlatformHazard_Mind>();
        our_physicality = GetComponent<BoxCollider>();
    }

    void Update()
    {
        our_physicality.enabled = our_mind.HowAreYouFeeling() is A06_LivingPlatformHazard_Mind.MOOD.WINTER;
    }

    //void OnTriggerEnter(Collider the_interloper)
    void OnCollisionEnter(Collision the_interloper)
    {
        if (our_mind.HowAreYouFeeling() is A06_LivingPlatformHazard_Mind.MOOD.WINTER && !our_satisfaction)
        {
            our_mind.ComeAgain();
            Destroy(our_mind.gameObject, 0.01f);
            our_satisfaction = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A06_LivingPlatformHazard_Mind : MonoBehaviour
{
    /// Our Concepts \\\
    const GameObject nobody = null;
    const int nothing = 0;
    const float a_moment = 1.0f;
    const float a_ways_above = 4.0f;
    const float a_leisurely_pace = 2.0f;
    const float a_brisk_pace = 20.0f;
    GameObject ourself;
    
    public enum MOOD
    {
        SPRING,
        SUMMER,
        AUTUMN,
        WINTER
    };

    /// Our Mind \\\
    MOOD our_feeling = MOOD.SPRING;
    GameObject our_concern = nobody;
    Vector3 our_last_concern;
    int our_regrets = nothing;
    float our_patience = a_moment;
    Vector3 our_home;
    public Sprite our_fair_visage;
    public Sprite our_fearsome_visage;
    public GameObject our_ideal_self;

    /// Our Body \\\
    BoxCollider our_sight;
    SpriteRenderer our_visage;
    HazardDamage our_mass;
    
    void Start()
    {
        ourself = gameObject;
        our_home = transform.position;
        our_sight = transform.Find("Our Sight").GetComponent<BoxCollider>();
        our_visage = transform.Find("Our Form/Eye").GetComponent<SpriteRenderer>();
        our_mass = transform.Find("Our Form/Tile").GetComponent<HazardDamage>();

        our_visage.sprite = our_fair_visage;
        our_mass.enabled = false;
        our_mass.SpawnParticlesHere.y = -1000;
    }
    
    void Update()
    {
        switch (our_feeling)
        {
            case MOOD.SPRING:
                break;
            case MOOD.SUMMER:
                break;
            case MOOD.AUTUMN:
                if (transform.position.y < our_home.y + a_ways_above)
                {
                    transform.position += Vector3.up * a_leisurely_pace * Time.deltaTime;
                    transform.right = (our_concern.transform.position - transform.position).normalized;
                }
                else if (our_patience > nothing)
                {
                    our_patience -= Time.deltaTime;

                    if (our_patience <= nothing)
                    {
                        our_feeling = MOOD.WINTER;
                        our_mass.enabled = true;
                        our_mass.tag = "Hazard";
                        our_mass.SpawnParticlesHere.y = 0;
                        our_last_concern = (our_concern.transform.position + Vector3.up * 0.5f - transform.position).normalized;
                    }
                }

                our_mass.transform.parent.localEulerAngles += new Vector3(0, 360.0f * Time.deltaTime, 0);
                break;
            case MOOD.WINTER:
                transform.position += our_last_concern * a_brisk_pace * Time.deltaTime;
                break;
        }
    }

    public void ComeAgain()
    {
        GameObject the_progeny = Instantiate(our_ideal_self, our_home, Quaternion.identity, transform.root);
        the_progeny.GetComponent<A06_LivingPlatformHazard_Mind>().our_ideal_self = our_ideal_self;
    }

    public void OnInterloperEnter(GameObject the_interloper)
    {
        if (the_interloper.transform.root.tag.Contains("Player"))
        {
            if (our_feeling == MOOD.SPRING)
            {
                our_concern = the_interloper.transform.root.GetChild(0).gameObject;
                our_feeling = MOOD.SUMMER;
                our_visage.sprite = our_fearsome_visage;
            }

            ++our_regrets;
        }
    }

    public void OnInterloperExit(GameObject the_interloper)
    {
        if (the_interloper.transform.root.tag.Contains("Player"))
        {
            --our_regrets;

            if (our_feeling is MOOD.SUMMER && our_regrets is nothing)
            {
                our_feeling = MOOD.AUTUMN;
            }
        }
    }

    public MOOD HowAreYouFeeling()
    {
        return our_feeling;
    }
}
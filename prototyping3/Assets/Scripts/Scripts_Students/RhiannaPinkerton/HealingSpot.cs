using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingSpot : MonoBehaviour
{
    [SerializeField] private GameObject BotParent;

    private bool isHealing = false;

    // Heal every X seconds
    [SerializeField] private float HealTimer = 2f;
    private float timer = 0f;

    [SerializeField] private float HealAmount = 1f;
    
    [SerializeField] private GameHandler Handler;
    
        [SerializeField] private GameObject HealParticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHealing)
        {
            HealParticles.transform.position = BotParent.GetComponentInChildren<BotBasic_Move>().transform.position;
            
            timer += Time.deltaTime;

            if (timer >= HealTimer)
            {
                timer = 0f;
                // Heal p1 or p2
                if (BotParent.tag.Equals("Player1"))
                {
                    // Can heal
                    if (GameHandler.p1Health < Handler.playersHealthStart)
                    {
                        // If adding to the health will go over the start health
                        if (Handler.playersHealthStart - GameHandler.p1Health < HealAmount)
                        {
                            GameHandler.p1Health = Handler.playersHealthStart;
                        }
                        else
                        {
                            Handler.TakeDamage(BotParent.tag, -HealAmount);
                        }
                    }
                }
                else // Player 2
                {
                    // Can heal
                    if (GameHandler.p2Health < Handler.playersHealthStart)
                    {
                        // If adding to the health will go over the start health
                        if (Handler.playersHealthStart - GameHandler.p2Health < HealAmount)
                        {
                            GameHandler.p2Health = Handler.playersHealthStart;
                        }
                        else
                        {
                            Handler.TakeDamage(BotParent.tag, -HealAmount);
                        }
                    }
                }

                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Don't start timer over if already healing
        if (other.transform.parent && BotParent.CompareTag(other.transform.parent.tag) && !isHealing)
        {
            HealParticles.transform.position = BotParent.GetComponentInChildren<BotBasic_Move>().transform.position;
            HealParticles.GetComponent<ParticleSystem>().Play();
            
            isHealing = true;
            timer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent && BotParent.CompareTag(other.transform.parent.tag))
        {
            HealParticles.GetComponent<ParticleSystem>().Stop();

            isHealing = false;
        }
    }
}

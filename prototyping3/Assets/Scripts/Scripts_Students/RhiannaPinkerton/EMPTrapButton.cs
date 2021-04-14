using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPTrapButton : MonoBehaviour
{
    public GameObject theButton;
    private float theButtonUpPos;
    
    private Vector3 targetPosition;
    
    [SerializeField] private GameObject EMPParticles;
    [SerializeField] private float FreezeTime = 0.5f;
    private float freezeTimer = 0f;
    
    private bool playerFrozen = false;

    private GameObject AffectedPlayer;

    [SerializeField] private float CooldownTime = 3f;
    private float cooldownTimer = 0f;
    
    private bool CooldownActive = false;

    private Renderer buttonRend;
    
    // Start is called before the first frame update
    void Start()
    {
        if (theButton != null){
            theButtonUpPos = theButton.transform.localPosition.y;
            buttonRend = theButton.GetComponent<Renderer>();
        }
    }

    private void Update()
    {
        if (playerFrozen)
        {
            freezeTimer += Time.deltaTime;
            if (freezeTimer >= FreezeTime)
            {
                freezeTimer = 0f;
                
                AffectedPlayer.GetComponentInChildren<BotBasic_Move>().enabled = true;
                EMPParticles.GetComponent<ParticleSystem>().Stop();

                playerFrozen = false;

                // Start cooldown
                CooldownActive = true;
                cooldownTimer = 0f;
                theButton.transform.localPosition = new Vector3(theButton.transform.localPosition.x, theButtonUpPos - 0.4f, theButton.transform.localPosition.z);
            }
        }
        else if (CooldownActive)
        {
            cooldownTimer += Time.deltaTime;

            Color c = Color.black;
            c.r = Mathf.Lerp(2f, 1f, cooldownTimer / CooldownTime);
            c.g = Mathf.Lerp(0.5f, 1f, cooldownTimer / CooldownTime);
            c.b = Mathf.Lerp(0.5f, 1f, cooldownTimer / CooldownTime);
            buttonRend.material.color = c;
            
            if (cooldownTimer >= CooldownTime)
            {
                CooldownActive = false;
                
                ButtonUp();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (playerFrozen || CooldownActive)
            return;
        
        if ((other.transform.root.gameObject.tag == "Player1"))
        {
            theButton.transform.localPosition = new Vector3(theButton.transform.localPosition.x, theButtonUpPos - 0.4f, theButton.transform.localPosition.z);
            buttonRend.material.color = new Color(2.0f, 0.5f, 0.5f, 2.5f);

            AffectedPlayer = GameObject.FindWithTag("Player2");
            AffectedPlayer.GetComponentInChildren<BotBasic_Move>().enabled = false;
            EMPParticles.transform.position = AffectedPlayer.GetComponentInChildren<BotBasic_Move>().transform.position;
            EMPParticles.GetComponent<ParticleSystem>().Play();
            playerFrozen = true;

        }
        else if (other.transform.root.gameObject.tag == "Player2")
        {
            theButton.transform.localPosition = new Vector3(theButton.transform.localPosition.x, theButtonUpPos - 0.4f, theButton.transform.localPosition.z);
            buttonRend.material.color = new Color(2.0f, 0.5f, 0.5f, 2.5f);

            AffectedPlayer = GameObject.FindWithTag("Player1");
            AffectedPlayer.GetComponentInChildren<BotBasic_Move>().enabled = false;
            EMPParticles.transform.position = AffectedPlayer.GetComponentInChildren<BotBasic_Move>().transform.position;
            EMPParticles.GetComponent<ParticleSystem>().Play();
            playerFrozen = true;
        }
    }
    
    public void ButtonUp(){
        theButton.transform.localPosition = new Vector3(theButton.transform.localPosition.x, theButtonUpPos, theButton.transform.localPosition.z);
        buttonRend.material.color = Color.white;
    }
}

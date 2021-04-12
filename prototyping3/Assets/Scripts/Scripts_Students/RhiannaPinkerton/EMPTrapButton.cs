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
    
    // Start is called before the first frame update
    void Start()
    {
        if (theButton != null){
            theButtonUpPos = theButton.transform.localPosition.y;
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

            }
        }
    }

    void FixedUpdate()
    {
        
        //stop the spawner
        //if (Spawner.transform.localPosition.y == pathEnd.transform.localPosition.y){
        if (playerFrozen == false){
            ButtonUp();
            //atStart = false;
            //playerFrozen = false;
        }
        //else if (Spawner.transform.localPosition.y == pathStart.transform.localPosition.y){
        //    ButtonUp();
        //    atStart = true;
        //    playerFrozen = false;
        //}
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (playerFrozen)
            return;
        
        if ((other.transform.root.gameObject.tag == "Player1"))
        {
            theButton.transform.localPosition = new Vector3(theButton.transform.localPosition.x, theButtonUpPos - 0.4f, theButton.transform.localPosition.z);
            //isMoving = true;
            Renderer buttonRend = theButton.GetComponent<Renderer>();
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
            //isMoving = true;
            Renderer buttonRend = theButton.GetComponent<Renderer>();
            buttonRend.material.color = new Color(2.0f, 0.5f, 0.5f, 2.5f);

            AffectedPlayer = GameObject.FindWithTag("Player1");
            AffectedPlayer.GetComponentInChildren<BotBasic_Move>().enabled = false;
            //EMPParticles.transform.position = AffectedPlayer.transform.position;
            EMPParticles.transform.position = AffectedPlayer.GetComponentInChildren<BotBasic_Move>().transform.position;
            EMPParticles.GetComponent<ParticleSystem>().Play();
            playerFrozen = true;
        }
    }
    
    public void ButtonUp(){
        //isMoving = false;
        theButton.transform.localPosition = new Vector3(theButton.transform.localPosition.x, theButtonUpPos, theButton.transform.localPosition.z);
        Renderer buttonRend = theButton.GetComponent<Renderer>();
        buttonRend.material.color = Color.white;
    }
}

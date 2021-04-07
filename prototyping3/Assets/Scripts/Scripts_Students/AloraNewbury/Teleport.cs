using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{


    public GameObject player;
    public AudioSource Begin;
    public AudioSource End;
    public ParticleSystem particle;
    public bool is_Teleporting;
    public float cooldown;
    public string button4; // currently boost in player move script




    Vector3 GetBehindlocation(float distance_behind)
    {

        if (GameObject.FindGameObjectWithTag("Player2").transform.GetChild(0).CompareTag("A05"))
        {
            Transform child = GameObject.FindGameObjectWithTag("Player1").transform.GetChild(0);

            if (child)
            {

                return child.transform.position - (child.transform.forward * distance_behind);
            }
        }

        if (GameObject.FindGameObjectWithTag("Player1").transform.GetChild(0).CompareTag("A05"))
        {
            Transform child = GameObject.FindGameObjectWithTag("Player2").transform.GetChild(0);

            if (child)
            {

                return child.transform.position - (child.transform.forward * distance_behind);
            }
        }


        return player.transform.position - (player.transform.forward * distance_behind);

    }


    // Start is called before the first frame update
    void Start()
    {
        button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

        cooldown = 0.0f;
        is_Teleporting = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown(button4) && cooldown <= 0.0f)
        {
            is_Teleporting = true;
            Begin.Play();
            particle.Play();

        }
        if (is_Teleporting == true && Begin.isPlaying == false)
        {
            player.transform.position = GetBehindlocation(5.0f);
            player.transform.position = player.transform.forward;

            End.Play();

            cooldown = 5.0f;

            is_Teleporting = false;
        }
        if (cooldown > 0.0f)
        {
            cooldown -= 0.1f;
        }

    }
}

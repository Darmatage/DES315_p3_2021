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


    Vector3 GetBehindlocation(float distance_behind)
    {

        if (GameObject.FindGameObjectWithTag("Player2"))
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
        cooldown = 0.0f;
        is_Teleporting = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Y) && cooldown <= 0.0f)
        {
            is_Teleporting = true;
            Begin.Play();
            particle.Play();

        }
        if (is_Teleporting == true && Begin.isPlaying == false)
        {
            player.transform.position = GetBehindlocation(5.0f);

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

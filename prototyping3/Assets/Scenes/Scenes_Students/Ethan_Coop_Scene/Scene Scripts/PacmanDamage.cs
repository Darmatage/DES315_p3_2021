using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanDamage : MonoBehaviour
{
    public float damage = 2f;
    public GameHandler GH;
    [SerializeField] float wait = 10f;
    [SerializeField] bool tookDamage = false, inContact = false;
    string playerContact;

    [SerializeField] PacManMove pacman;
    // Start is called before the first frame update
    void Start()
    {
        GH = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        pacman = transform.parent.gameObject.GetComponent<PacManMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if(tookDamage)
        {
            wait -= Time.deltaTime;
        }
        if(wait <= 0)
        {
            tookDamage = false;
            wait = 10;
        }

        if(inContact && !tookDamage && pacman.attack)
        {
            GH.TakeDamage(playerContact, damage);
            tookDamage = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.transform.root.tag == "Player1" || other.transform.root.tag == "Player2") && pacman.attack)
        {
            inContact = true;
            playerContact = other.transform.root.tag;
            GH.TakeDamage(playerContact, damage);
            tookDamage = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        inContact = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.root.tag == "Player1" || collision.transform.root.tag == "Player2")
        {
            GH.TakeDamage(collision.transform.root.tag, damage);
        }
    }
}

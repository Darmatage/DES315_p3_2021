using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    public GameHandler gameHandler;
    public GameObject ball;
    public float health;
    public Color collideColor;
    public Color normalColor;
    public Color winColor;

    // Start is called before the first frame update
    void Start()
    {
        health = 20.0f;
        gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0.0f)
        {
            //Destroy(ball);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (health > 0.0f)
        {
            if ((other.gameObject.tag == "Hazard") && (other.gameObject.GetComponent<HazardDamage>().isMonsterWeapon == false))
            //if (other.gameObject.transform.parent.tag == "Player1" || other.gameObject.transform.parent.tag == "Player2")
            {
                health -= 2.0f;
                gameHandler.CoopUpdateMonster(health);

                StartCoroutine(thing());
               
            }
        }
        else
        {
            GetComponent<Renderer>().material.color = winColor;

        }

    }

    IEnumerator thing()
    {
        GetComponent<Renderer>().material.color = collideColor;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Renderer>().material.color = normalColor;

    }

}

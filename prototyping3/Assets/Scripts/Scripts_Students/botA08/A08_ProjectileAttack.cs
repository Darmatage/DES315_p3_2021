using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A08_ProjectileAttack : MonoBehaviour
{
    public float cooldown = 5.0f;
    public float projectileVelocity = 10.0f;
    public int damageCost = 3;
    public GameObject projectile;

    private float cooldownTimer = 0;
    private GameHandler gameHandler;

    //grab axis from parent object
    public string button1;
    public string button2;
    public string button3;
    public string button4; // currently boost in player move script


    void Start()
    {
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

        if (GameObject.FindWithTag("GameHandler") != null)
        {
            gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        }

    }


    void Update()
    {
        if ((Input.GetButtonDown(button2)) && cooldownTimer <= 0)
        {
            GameObject shot = Instantiate(projectile, this.transform.position, Quaternion.identity);

            if (this.transform.root.tag == "Player1")
            { 
                shot.transform.GetChild(0).GetComponent<HazardDamage>().isPlayer1Weapon = true;
            }
            else
            { 
                shot.transform.GetChild(0).GetComponent<HazardDamage>().isPlayer2Weapon = true;
            }

            shot.GetComponent<Rigidbody>().AddForce(this.transform.forward * projectileVelocity);
            shot.GetComponent<Rigidbody>().AddTorque(transform.up * 1000);

            gameHandler.TakeDamage(this.transform.root.tag, damageCost);

            cooldownTimer = cooldown;
        }

        cooldownTimer -= Time.deltaTime;
    }

}

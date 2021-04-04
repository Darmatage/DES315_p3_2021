using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamage_AmyS : MonoBehaviour
{
    public static float totalHealth;
    public float healthStart = 30f;

    private GameHandler gameHandler;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindWithTag("GameHandler") != null)
        {
            gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        }

        totalHealth = healthStart;
        gameHandler.CoopUpdateMonster(totalHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Hazard"))
        {
            TakeDamage(other.gameObject.GetComponent<HazardDamage>().damage);
        }
    }

    void TakeDamage(float amount)
    {
        totalHealth -= amount;

        if(totalHealth <= 0)
        {
            totalHealth = 0;
        }

        gameHandler.CoopUpdateMonster(totalHealth);
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}

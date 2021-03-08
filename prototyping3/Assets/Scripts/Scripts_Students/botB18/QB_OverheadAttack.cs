using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QB_OverheadAttack : MonoBehaviour
{
    public float attackCooldown;
    public float retractionCooldown;

    public string attackKey;


    private float attackTimer = 0.0f;
    private float retractionTimer = 0.0f;

    private bool isDown = false;
    private bool canAttack = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDown)
        {
            retractionTimer -= Time.deltaTime;

            if(retractionTimer <= 0.0f)
            {
                Vector3 rot = new Vector3(-56.0f, 0.0f);
                //rot.x = -56.0f;
                transform.localRotation = Quaternion.Euler(rot);

                isDown = false;
                retractionTimer = 0.0f;
            }
        }
        else if(Input.GetButtonDown(attackKey) && canAttack)
        {
            Vector3 rot = new Vector3(72.0f, 0.0f);
            //rot.x = 72.0f;
            transform.localRotation = Quaternion.Euler(rot);

            retractionTimer = retractionCooldown;
            attackTimer = attackCooldown;
            isDown = true;
            canAttack = false;
        }

        if(attackTimer > 0.0f)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer <= 0.0f)
            {
                canAttack = true;
                attackTimer = 0.0f;
            }
        }
    }
}

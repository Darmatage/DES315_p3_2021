using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QB_OverheadAttack : MonoBehaviour
{
    public float attackCooldown;
    public float retractionCooldown;
    public float angularVelocity;

    public string attackKey;


    private float attackTimer = 0.0f;
    private float retractionTimer = 0.0f;

    private bool isDown = false;
    private bool isAttacking = false;
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
                //Vector3 rot = new Vector3(-56.0f, 0.0f);
                //rot.x = -56.0f;
                //transform.localRotation = Quaternion.Euler(rot);

                Vector3 rot = new Vector3(-angularVelocity * Time.deltaTime, 0.0f);
                rot.x += transform.localRotation.eulerAngles.x;
                transform.localRotation = Quaternion.Euler(rot);

                if (!(rot.x >= (360.0f - 58.0f) || rot.x <= 75.0f))
                {
                    isDown = false;
                    retractionTimer = 0.0f;
                }
                return;
            }
        }
        else if(Input.GetButtonDown(attackKey) && canAttack)
        {
            //Vector3 rot = new Vector3(72.0f, 0.0f);
            //rot.x = 72.0f;
            //transform.localRotation = Quaternion.Euler(rot);

            //retractionTimer = retractionCooldown;
            //attackTimer = attackCooldown;
            //isDown = true;
            isAttacking = true;
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

        if(isAttacking)
        {
            Vector3 rot = new Vector3(angularVelocity * Time.deltaTime, 0.0f);
            rot.x += transform.localRotation.eulerAngles.x;
            transform.localRotation = Quaternion.Euler(rot);

            //if(rot.x >= 72.0f && rot.x < (360.0f - 56.0f))
            //{
            //    isAttacking = false;
            //    isDown = true;
            //    attackTimer = attackCooldown;
            //    retractionTimer = retractionCooldown;
            //}
        }
        //else
        //{
        //    Vector3 rot = transform.localEulerAngles;
        //    if(rot.x >= -56.0f)
        //    {
        //        rot.x -= angularVelocity * Time.deltaTime;
        //        transform.localRotation = Quaternion.Euler(rot);
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        isAttacking = false;
        isDown = true;
        attackTimer = attackCooldown;
        retractionTimer = retractionCooldown;
    }
}

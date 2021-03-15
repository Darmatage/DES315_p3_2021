using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QB_FlipAttack : MonoBehaviour
{
    public float attackCooldown;
    //public float retractionCooldown;

    public string attackKey;

    //public MeshCollider collider;
    
    private float attackTimer = 0.0f;
    //private float retractionTimer = 0.0f;

    //private bool isUp = false;
    private bool canAttack = true;

    private ArrayList flipObjs;

    // Start is called before the first frame update
    void Start()
    {
        flipObjs = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(attackKey) && canAttack)
        {
            canAttack = false;
            attackTimer = attackCooldown;

            foreach(Object obj in flipObjs)
            {
                ((GameObject)obj).GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 10.0f);
                ((GameObject)obj).GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));
            }
        }

        if (attackTimer > 0.0f)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0.0f)
            {
                canAttack = true;
                attackTimer = 0.0f;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.root.tag != transform.root.tag && other.gameObject.transform.root == other.gameObject.transform.parent)
        {
            flipObjs.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.root.tag != transform.root.tag && other.gameObject.transform.root == other.gameObject.transform.parent)
        {
            flipObjs.Remove(other.gameObject);
        }
    }
}

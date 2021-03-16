using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehavior : MonoBehaviour
{
    [SerializeField] private float Speed;

    public Vector3 Target;
    private Vector3 StartPos;
    
    [SerializeField] private GameObject particlesPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        GetComponent<Rigidbody>().AddForce((Target - transform.position) * Speed, ForceMode.Impulse);
    }

    private void Update()
    {
        float dist = Vector3.Distance(StartPos, transform.position);
        if (dist >= 8f)
        {
            GameObject damageParticles = Instantiate (particlesPrefab, transform.position, Quaternion.identity);
            StartCoroutine(destroyParticles(damageParticles));
            Destroy(gameObject);
        }
    }
    
    IEnumerator destroyParticles(GameObject particles){
        yield return new WaitForSeconds(0.5f);
        Destroy(particles);
    }
    
    void OnCollisionEnter(Collision other){
        if (other.gameObject.transform.root.tag.Contains("Player"))
        {
            float KnockBackStrength = 10f;
            Vector3 direction = other.transform.position - transform.position;
            Vector3 velocity = direction.normalized * KnockBackStrength + (Vector3.up * (KnockBackStrength / 3f));
            direction.y = 3.0f;
            other.gameObject.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
        }
        Destroy(gameObject);
    }
}

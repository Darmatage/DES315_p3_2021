using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotB09_Boulder : MonoBehaviour
{
    public float boulderHealth;
    
    public GameObject particlesPrefab;
    public Vector3 SpawnParticlesHere;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boulderHealth <= 0)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collided with " + collision.gameObject.name);
        Debug.Log("has the tag" + collision.gameObject.tag);
        if(collision.collider.CompareTag("Hazard"))
        {
            if(collision.collider.GetComponent<HazardDamage>() != null)
            {
                SpawnParticlesHere = collision.contacts[0].point;
                boulderHealth -= collision.collider.GetComponent<HazardDamage>().damage;
                GameObject damageParticles = Instantiate(particlesPrefab, SpawnParticlesHere, collision.transform.rotation);
                StartCoroutine(destroyParticles(damageParticles));
            }
        }
    }
    IEnumerator destroyParticles(GameObject particles)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(particles);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot09_Damage : MonoBehaviour
{
    public GameObject particlesPrefab;
    public AudioSource damagesound;
    public Vector3 SpawnParticlesHere;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("whho[ps");
        SpawnParticlesHere = other.contacts[0].point;
        //make particles
        GameObject damageParticles = Instantiate(particlesPrefab, SpawnParticlesHere, other.transform.rotation);
        damageParticles.GetComponent<ParticleSystem>().Play();
        damagesound.Play();
        StartCoroutine(destroyParticles(damageParticles));
    }

    IEnumerator destroyParticles(GameObject particles)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(particles);
    }

}

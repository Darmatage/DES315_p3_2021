using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehavior : MonoBehaviour
{

    [SerializeField] private float SpawnTime;
    private float spawnTimer = 0f;

    [SerializeField] private float LifeTime;
    private float lifeTimer = 0f;

    [SerializeField] private float BlinkIntervalTime = 1f;
    [SerializeField] private float BlinkDecay = 0.1f;
    private float blinkTimer = 0f;
    [SerializeField] private float BlinkHoldTime = 0.1f;
    private float blinkHoldTimer = 0f;
    
    
    // If bomb is active
    private bool isActive = false;


    [SerializeField] private Material InactiveColor;
    [SerializeField] private Material ActiveColor;
    [SerializeField] private Material BlinkColor;

    [SerializeField] private MeshRenderer Rend;

    private bool isBlinking = false;

    [SerializeField] private GameObject MineObj;
    //[SerializeField] private GameObject ExplosionObj;

    private BoxCollider Collider;
    
    [SerializeField] private GameObject particlesPrefab;

    public MineSpawner ParentSpawner;
    
    // Start is called before the first frame update
    void Start()
    {
        Rend.material = InactiveColor;
        Collider = GetComponent<BoxCollider>();
        Collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn timer is the time before mine is active
        if (spawnTimer >= SpawnTime && !isActive)
        {
            isActive = true;
            gameObject.tag = "Hazard";
            Rend.material = ActiveColor;
            Collider.enabled = true;
        }
        else
        {
            spawnTimer += Time.deltaTime;
        }
        
        if (isActive)
        {
            // Not currently doing a blink
            if (blinkHoldTimer <= 0f)
            {
                // Regular blink timer
                blinkTimer += Time.deltaTime;
                if (blinkTimer >= BlinkIntervalTime)
                {
                    // Shorten time for next blink
                    BlinkIntervalTime -= BlinkDecay;
                    // Start active blinking
                    blinkHoldTimer += Time.deltaTime;
                    Rend.material = BlinkColor;
                    //isBlinking = true;
                }
            }
            else
            {
                blinkHoldTimer += Time.deltaTime;
                if (blinkHoldTimer >= BlinkHoldTime)
                {
                    blinkHoldTimer = 0f;
                    blinkTimer = 0f;
                    Rend.material = ActiveColor;
                    //isBlinking = false;
                }
            }

        }

        // Life timer after it's been active
        if (lifeTimer >= LifeTime)
        {
            GameObject damageParticles = Instantiate (particlesPrefab, transform.position, Quaternion.identity);
            StartCoroutine(destroyParticles(damageParticles));
            
            
            Explode();
        }
        else if (isActive)
        {
            lifeTimer += Time.deltaTime;
        }
        
        
    }
    
    // Collision code to check if isActive
    //private void OnTriggerEnter(Collider other)
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Contains("ground"))
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }
        
        else if (isActive && other.gameObject.transform.root.tag.Contains("Player"))
        {
            float KnockBackStrength = 10f;
            Vector3 direction = other.transform.position - transform.position;
            Vector3 velocity = direction.normalized * KnockBackStrength + (Vector3.up * (KnockBackStrength / 3f));
            direction.y = 3.0f;
            other.gameObject.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
            Explode();
        }
    }

    private void Explode()
    {
        ParentSpawner.IncrementMines();
        MineObj.SetActive(false);
        isActive = false;
        Collider.enabled = false;
        Destroy(gameObject);
    }

    IEnumerator destroyParticles(GameObject particles){
        yield return new WaitForSeconds(0.5f);
        Destroy(particles);
    }
}


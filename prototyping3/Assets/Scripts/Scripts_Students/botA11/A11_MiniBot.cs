using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Amogh
{
    public class A11_MiniBot : MonoBehaviour
    {
        private Transform dad;
        private NavMeshAgent agent;

        [SerializeField] private float radius;
        [SerializeField] private float health;

        [SerializeField] private GameObject notes;
        
        [SerializeField] private AudioClip healsSong;
        private AudioSource source;

        private GameObject ground;
        private Rigidbody rb;
        private bool grounded;
        
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            source = GetComponent<AudioSource>();
            source.clip = healsSong;

            source.pitch = 1f;
            source.Play();

            ground = GameObject.Find("Ground");
            rb = GetComponent<Rigidbody>();
            
            StartCoroutine(SpawnNotes());
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        IEnumerator SpawnNotes()
        {
            while (true)
            {
                for (int i = 0; i < 2; ++i)
                {
                    GameObject note = Instantiate(notes, transform.position + (transform.up * 4) + transform.right, Quaternion.identity);
                    note.GetComponent<A11_Notes>().SetTrackingTransform(dad);
                    
                    yield return new WaitForSeconds(0.5f);
                }

                yield return new WaitForSeconds(1f);
            }
        }
        
        public void SetTrackingTransform(Transform t)
        {
            dad = t;
            
            InvokeRepeating(nameof(SetDestinationWithinSphere), 0.5f, 1f);
        }

        private void SetDestinationWithinSphere()
        {
            Vector3 randomPos = Random.insideUnitSphere * radius + dad.position;
            
            grounded = false;
            if (agent.enabled)
            {
                agent.SetDestination(randomPos);
                agent.updatePosition = false;
                agent.updateRotation = false;
                agent.isStopped = true;
            }
            
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddRelativeForce(new Vector3(0, 20f, 0), ForceMode.Impulse);
        }

        private void ChangePitch()
        {
            source.pitch -= Time.deltaTime;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Hazard") || (other.gameObject.tag.Contains("Player") && other.gameObject != dad.root.gameObject))
            {
                InvokeRepeating(nameof(ChangePitch), 0.1f, 0.1f);
                Destroy(gameObject, 0.5f);
                dad.gameObject.GetComponent<A11_Heal>().MiniBotDeath();
            }
            else if (other.gameObject.Equals(ground))
            {
                if (grounded == false)
                {
                    if (agent.enabled)
                    {
                        agent.updatePosition = true;
                        agent.updateRotation = true;
                        agent.isStopped = false;
                    }
                    rb.isKinematic = true;
                    rb.useGravity = false;
                    grounded = true;
                }
            }
        }
    }
}
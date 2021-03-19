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

        [SerializeField] private GameObject[] notes;
        
        [SerializeField] private AudioClip healsSong;
        private AudioSource source;
        
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            source = GetComponent<AudioSource>();
            source.clip = healsSong;

            source.pitch = 1f;
            source.Play();

            StartCoroutine(SpawnNotes());
            
            // Self destruct
            Destroy(gameObject, 20f);
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
                    GameObject note = Instantiate(notes[Random.Range(0,2)], transform.position + (transform.up * 4) + transform.right, Quaternion.identity);
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
            
            agent.SetDestination(randomPos);
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
            
        }
    }
}
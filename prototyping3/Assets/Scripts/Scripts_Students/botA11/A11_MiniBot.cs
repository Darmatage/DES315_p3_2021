using System;
using System.Collections;
using System.Collections.Generic;
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
        
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            
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

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Hazard") || (other.gameObject.tag.Contains("Player") && !other.gameObject.CompareTag(dad.root.tag)))
            {
                Destroy(gameObject);
                dad.gameObject.GetComponent<A11_Heal>().MiniBotDeath();
            }
        }
    }
}
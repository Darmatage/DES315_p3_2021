using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Amogh
{
    public class A11_Notes : MonoBehaviour
    {
        private static Transform dad;

        [SerializeField] private float radius = 5f; 
        
        private NavMeshAgent agent;

        private static GameHandler handler;

        private Vector3 targetPos;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            //agent.updatePosition = false;

            if (handler == null)
            {
                handler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
            }
            
            InvokeRepeating(nameof(Spin), 0.3f, 0.2f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void SetAgentDestination()
        {
            agent.Warp(Vector3.MoveTowards(transform.position, targetPos, 1f));
            
            //agent.SetDestination(dad.position);
        }
        
        private void SetTargetWithinSphere()
        {
            targetPos = Random.insideUnitSphere * radius + dad.position;
            //agent.Warp(Vector3.MoveTowards(transform.position, randomPos, 1f));
        }
        
        private void Spin()
        {
            transform.Rotate(Vector3.up, 10f);
        }
        
        public void SetTrackingTransform(Transform t)
        {
            dad = t;
            targetPos = dad.position;
            
            InvokeRepeating(nameof(SetAgentDestination), 0.9f, 0.5f);
            InvokeRepeating(nameof(SetTargetWithinSphere), 0.5f, 1.0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.CompareTag("Hazard"))
            {
                Destroy(gameObject);
            } 
            else if (other.gameObject.name.Contains("Bot"))
            {
                handler.TakeDamage(other.gameObject.transform.root.tag, -1);
                Destroy(gameObject);
            }
        }
    }
    
}

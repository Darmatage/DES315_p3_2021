using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BotB04.Controller
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 5f;
        private Rigidbody rb;
        private HazardDamage hazard;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * speed);

            hazard = GetComponent<HazardDamage>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.tag == "Player1") 
            {
                if(!hazard.isPlayer1Weapon)
                    Impact();
            }
            else if (other.transform.root.tag == "Player2") 
            {
                if (!hazard.isPlayer2Weapon)
                    Impact();
            }
            else
            {
                Impact();
            }
        }


        private void Impact()
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustinStead.Vent
{
    public class AustinStead_VentCollision : MonoBehaviour
    {
        //Public knobs
        public float Acceleration = 3f;
        public float MaxYSpeed = 10f;
        public float NegativeYSpeedMod = 2f;
        //Public refs
        public AustinStead_Button Button;
        //Private refs
        private MeshCollider collider;

        private AustinStead_VentParent ventBase;


        void Start()
        {
            collider = GetComponent<MeshCollider>();
            ventBase = transform.parent.GetComponent<AustinStead_VentParent>();
            collider.enabled = ventBase.Enabled;
        }

        private void OnEnable()
        {
             if(ventBase == null)
                ventBase = transform.parent.GetComponent<AustinStead_VentParent>();

            ventBase.VentActivate += BaseActivation;
        }
        private void OnDisable()
        {
            ventBase.VentActivate -= BaseActivation;
        }




        private void BaseActivation(bool state)
        {
            collider.enabled = state;
        }


        //Collision------------------------------------------------------------
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.tag == "Player1" || other.transform.root.tag == "Player2")
            {
                SetGravity(other.transform.root.gameObject, true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.tag == "Player1" || other.transform.root.tag == "Player2")
            {
                SetGravity(other.transform.root.gameObject, false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.transform.root.tag == "Player1" || other.transform.root.tag == "Player2")
            {
                UpdateGravity(other.transform.root.gameObject);
            }
        }


        private void SetGravity(GameObject root, bool enter)
        {
            Rigidbody[] rbList = root.GetComponentsInChildren<Rigidbody>(false);
            //Rigidbody rb = GetComponentInChildren<Rigidbody>(false);
            if (enter)
            {
                foreach (Rigidbody rb in rbList)
                {
                    rb.useGravity = false;
                }
            }
            else
            {
                foreach (Rigidbody rb in rbList)
                {
                    rb.useGravity = true;
                }
            }
        }


        private void UpdateGravity(GameObject root)
        {
            Rigidbody[] rbList = root.GetComponentsInChildren<Rigidbody>(false);
            //Rigidbody rb = GetComponentInChildren<Rigidbody>(false);

            foreach (Rigidbody rb in rbList)
            {
                if (rb.velocity.y < 0)
                    rb.velocity = rb.velocity + transform.up * Acceleration * NegativeYSpeedMod * Time.deltaTime;
                else
                    rb.velocity = rb.velocity + transform.up * Acceleration * Time.deltaTime;

                if (rb.velocity.y > MaxYSpeed)
                    rb.velocity = new Vector3(rb.velocity.x, MaxYSpeed, rb.velocity.z);

                rb.angularVelocity = new Vector3();
                rb.transform.rotation = new Quaternion(0, rb.transform.rotation.y, 0, rb.transform.rotation.w);
            }

        }

    }
}

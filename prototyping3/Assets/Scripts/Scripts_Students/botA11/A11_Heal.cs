using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Amogh
{
    public class A11_Heal : MonoBehaviour, A11_IAttack
    {
        [SerializeField] private GameObject miniBotPrefab;

        private GameObject miniBotClone;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ButtonDown()
        {
            if (miniBotClone)
                return;
            
            Vector3 pos = transform.position + transform.right;

            miniBotClone = Instantiate(miniBotPrefab, pos, Quaternion.identity);
            miniBotClone.GetComponent<Rigidbody>().AddForce(transform.right * 2);
            miniBotClone.GetComponent<A11_MiniBot>().SetTrackingTransform(transform);
        }

        public void ButtonHeld()
        {
            
        }

        public void ButtonUp()
        {
            
        }

        public void MiniBotDeath()
        {
            miniBotClone = null;
        }
    }
}
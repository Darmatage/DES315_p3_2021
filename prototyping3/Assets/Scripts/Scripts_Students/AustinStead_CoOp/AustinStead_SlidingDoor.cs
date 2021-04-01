using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustinStead
{

    public class AustinStead_SlidingDoor : MonoBehaviour
    {
        public bool IsOpen = false;

        public AustinStead_Button Button;

        public GameObject door;

        private Animator doorAnim;

        public Material OpenMat;
        public Material ClosedMat;

        private Renderer renderer;

        private void OnEnable()
        {
            Button.ButtonActivated += Activate;
        }
        private void OnDisable()
        {
            Button.ButtonActivated -= Activate;
        }

        private void Start()
        {
            doorAnim = door.GetComponent<Animator>();
            renderer = door.GetComponent<Renderer>();

            //doorAnim.SetBool("IsOpen", IsOpen);
            IsOpen = !IsOpen;
            Activate();
        }

        void Activate()
        {

            IsOpen = !IsOpen;
            doorAnim.SetBool("IsOpen", IsOpen);

            Debug.Log("IsOpen = " + IsOpen);

            if (IsOpen)
            {
                renderer.material = OpenMat;
            }
            else
            {
                renderer.material = ClosedMat;
            }

        }


    }
}


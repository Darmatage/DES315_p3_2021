using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustinStead.Vent
{
    public class AustinStead_VentParent : MonoBehaviour
    {
        public delegate void ActivateAction(bool activate);
        public event ActivateAction VentActivate;

        public AustinStead_Button Button;
        public bool Enabled = true;

        public GameObject VentBase;
        private Renderer ventBaseMat;
        private ParticleSystem activeParticles;

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
            activeParticles = GetComponent<ParticleSystem>();
            Temp();
        }

        void Activate()
        {
            Enabled = !Enabled;
            Temp();
        }


        void Temp()
        {
            if (Enabled)
                activeParticles.Play();
            else
                activeParticles.Stop();

            VentActivate(Enabled);
        }

    }
}

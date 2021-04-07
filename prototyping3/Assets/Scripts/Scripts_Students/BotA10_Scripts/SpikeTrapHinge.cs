using UnityEngine;

namespace Scripts_Students.BotA10_Scripts
{
    public class SpikeTrapHinge : MonoBehaviour
    {
        [SerializeField] private float startRotation = 0.0f;
        [SerializeField] private float triggeredRotation = 90.0f;
        [SerializeField] private float spring = 15000.0f;
        private HingeJoint _hinge;
        private bool _activated;

        private void Start()
        {
            _hinge = GetComponent<HingeJoint>();
            _hinge.useSpring = true;
        }

        private void Update()
        {
            var jointSpring = new JointSpring
            { spring = spring, targetPosition = _activated ? triggeredRotation : startRotation };
            _hinge.spring = jointSpring;
            _hinge.useLimits = true;
        }

        public void TriggerOn() => _activated = true;
        public void TriggerOff() => _activated = false;
    }
}

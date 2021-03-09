using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A09
{
    public class TransformationController : MonoBehaviour
    {
        public BotState State { get; private set; }
        public BotState NextState { get; private set; }

        public event Action TransformationStarted;
        public event Action TransformationEnded;

        [SerializeField] private float rotateSpeed;
        [SerializeField] private ExtendableBotPart[] bodyParts;
        [SerializeField] private BotBasic_Move movement;
        [SerializeField] private Barrel barrel;
        
        private string _transformButton, _attackButton, _horizontal;

        private void OnValidate()
        {
            if (bodyParts == null)
                bodyParts = GetComponentsInChildren<ExtendableBotPart>();

            if (movement == null)
                movement = GetComponent<BotBasic_Move>();

            if (barrel == null)
                barrel = GetComponentInChildren<Barrel>();
        }

        private void Awake()
        {
            State = BotState.Roaming;
            NextState = State;
        }

        private void Start()
        {
            _horizontal = gameObject.transform.parent.GetComponent<playerParent>().rotateAxis;
            _transformButton = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
            _attackButton = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        }

        private void Update()
        {
            if (Input.GetButtonDown(_transformButton) && State != BotState.Transforming)
            {
                ToggleState();
            }

            if (State == BotState.Sitting)
            {
                if (Input.GetButton(_attackButton))
                    barrel.Fire();
                
                float botRotate = Input.GetAxisRaw(_horizontal) * rotateSpeed * Time.deltaTime;
                transform.Rotate(0, botRotate, 0);
            }
        }

        private void ToggleState()
        {
            TransformationStarted?.Invoke();
            StartCoroutine(State == BotState.Roaming ? DoSitTransformation() : DoRoamTransformation());
        }

        private IEnumerator DoSitTransformation()
        {
            State = BotState.Transforming;
            NextState = BotState.Sitting;
            movement.enabled = false;
            
            foreach (var part in bodyParts)
                part.Extend();

            yield return new WaitUntil(() =>
            {
                var allExtended = true;

                foreach (var part in bodyParts)
                    if (part.IsMoving)
                        allExtended = false;

                return allExtended;
            });

            State = NextState;
        }

        private IEnumerator DoRoamTransformation()
        {
            State = BotState.Transforming;
            NextState = BotState.Roaming;
            movement.enabled = false;
            
            foreach (var part in bodyParts)
                part.Retract();
            
            yield return new WaitUntil(() =>
            {
                var allRetracted = true;

                foreach (var part in bodyParts)
                    if (part.IsMoving)
                        allRetracted = false;

                return allRetracted;
            });

            State = NextState;
            movement.enabled = true;
        }
    }
}

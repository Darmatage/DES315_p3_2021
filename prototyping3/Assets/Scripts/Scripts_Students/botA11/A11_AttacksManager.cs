using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Amogh
{
    public class A11_AttacksManager : MonoBehaviour
    {
        private string[] buttonCodes;

        private A11_IAttack[] attacks;
        
        // Start is called before the first frame update
        void Start()
        {
            var parentScript = GetComponentInParent<playerParent>();

            buttonCodes = new[] { 
                parentScript.action1Input, 
                parentScript.action2Input, 
                parentScript.action3Input, 
                parentScript.action4Input 
            };

            attacks = new[]
            {
                GetComponent<A11_Heal>().GetComponent<A11_IAttack>(),
                null,
                null,
                null
            };

        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < buttonCodes.Length; ++i)
            {
                if (Input.GetButtonDown(buttonCodes[i]))
                {
                    if (attacks[i] != null)
                    {
                        attacks[i].ButtonDown();
                    }
                }
                else if (Input.GetButton(buttonCodes[i]))
                {
                    if (attacks[i] != null)
                    {
                        attacks[i].ButtonHeld();
                    }
                }
                else if (Input.GetButtonUp(buttonCodes[i]))
                {
                    if (attacks[i] != null)
                    {
                        attacks[i].ButtonUp();
                    }
                }
            }
        }
    }
}

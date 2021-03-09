using System;
using UnityEngine;

namespace Scripts_Students.BotA10_Scripts
{
    public class BotA10_Weapon : MonoBehaviour
    {
        //NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot


        //grab axis from parent object
        public string button1;
        public string button2;
        public string button3;
        public string button4; // currently boost in player move script

        public event EventHandler OnShootB1;
        public bool isPlayer1;
        public void Start()
        {
            button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
            button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
            button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
            button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;
            
            isPlayer1 = gameObject.transform.root.GetComponent<playerParent>().isPlayer1;
        }

        private void Update()
        {
            if (Input.GetButtonDown(button1))
            {
                OnShootB1?.Invoke(this, EventArgs.Empty);
            }
        }
        
    }
}
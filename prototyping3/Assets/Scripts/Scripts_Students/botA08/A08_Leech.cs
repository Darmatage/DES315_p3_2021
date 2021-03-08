using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A08_Leech : MonoBehaviour
{
    GameHandler gameHandler;
    public int healthRegain = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindWithTag("GameHandler") != null)
        {
            gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag != this.transform.root.tag)
        {
            if (other.transform.root.tag == "Player1" || other.transform.root.tag == "Player2")
            {
                gameHandler.TakeDamage(this.transform.root.tag, -healthRegain);
            }

        }
    }
}

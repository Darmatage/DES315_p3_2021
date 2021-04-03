using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AkshatMadan_GoalScript : MonoBehaviour
{
    public bool isGoal2 = false;
    public GameObject goalsText;
    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoal2)
            goalsText.GetComponent<Text>().text = "P1 Goals: " + score;
        else
            goalsText.GetComponent<Text>().text = "P2 Goals: " + score;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            // Goal scored
            score++;
        }
    }
}

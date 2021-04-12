using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{

    public GameObject lava;
    public GameHandler timerObj;
    public AudioSource bubbling;
    public int gameTime = 120;
    public GameObject fight;
    private float gameTimer = 0f;
    private bool isGameTime = false;
    public bool scaled;



    // Start is called before the first frame update
    void Start()
    {
        fight = timerObj.fightButton; 
        scaled = false;
        isGameTime = false;
        bubbling.Play();

    }

    void FixedUpdate()
    {
        if (isGameTime == true)
        {
            gameTimer += 0.01f;
            if (gameTime <= 0)
            {
                gameTime = 0;
         
            }
            else if ((gameTimer >= 1f))
            {
                gameTime -= 1;
                gameTimer = 0;
            }
        }
    }


    void Update()
    {
        if (fight.activeSelf == true)
        {
            isGameTime = true;
        }

        if ((int)gameTimer % 10 == 0 && gameTimer > 0 )
        {
            if (scaled == false)
            {
                scaled = true;
                ChangeSize(true);
            }

        }
        else
        {
            ChangeSize(false);
            scaled = false;
        }
    }

    public void ChangeSize(bool bigger)
    {

        if (bigger && scaled)
        {
            lava.transform.localScale += new Vector3(0, 2, 0);
        }
        


    }


}

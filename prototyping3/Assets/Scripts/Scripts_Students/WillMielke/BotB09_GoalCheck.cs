using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class BotB09_GoalCheck : MonoBehaviour
{
    public bool p1entered, p2entered;
    public static bool wonGame = false;
    // Start is called before the first frame update

    void Awake()
    {
        SceneManager.sceneUnloaded += BotB09WinText;
    }
    
    
    void Start()
    {
        p1entered = false;
        p2entered = false;
        wonGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (p1entered && p2entered)
        {
            // won
            wonGame = true;
            StartCoroutine(WonGame());

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!p1entered && collision.transform.parent.gameObject.CompareTag("Player1"))
        {
            p1entered = true;
        }
        else if (!p2entered && collision.transform.parent.gameObject.CompareTag("Player2"))
        {
            p2entered = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (p1entered && collision.transform.parent.gameObject.CompareTag("Player1"))
        {
            p1entered = false;
        }
        else if (p2entered && collision.transform.parent.gameObject.CompareTag("Player2"))
        {
            p2entered = false;
        }
    }

    IEnumerator WonGame()
    {
        GameHandler.notFirstGame = true;
        yield return new WaitForSeconds(1.0f);
        GameHandler.winner = "Your team beat the obstacle course!";
        SceneManager.LoadScene("EndSceneCoop");
    }

    void BotB09WinText(Scene scene)
    {
        if(scene.name == "Scene_BotB09" && !wonGame)
        {
            GameHandler.winner = "Obstacle Course Won!";
        }
            
    }


}

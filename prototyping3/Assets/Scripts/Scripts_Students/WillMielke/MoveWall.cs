using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour
{
    public GameObject Wall;
    public BotB09_WallButton button;
    public Transform movePosition;

    public Vector3 initialpos;
    public float duration = 1.0f;
    public float respawnDuration = 5.0f;
    public bool teleportBack;
    public Collider buttonCollider;
    private float t;
    // Start is called before the first frame update
    void Start()
    {
        t = 0.0f;
        initialpos = Wall.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(button.move)
        {
            
            t += Time.deltaTime;
            if (t > duration)
            {
                t = duration;
                Wall.transform.position = movePosition.position;
                if(teleportBack)
                {
                    buttonCollider.enabled = false;
                    teleportBack = false;
                    StartCoroutine(ResetPos());
                }
            }
                
        }
        else
        {
            t -= Time.deltaTime;
            if (t < 0.0f)
            {
                t = 0.0f;
                Wall.transform.position = initialpos;
            }
                
        }
        if(t > 0.0f && t < duration)
            Wall.transform.position = initialpos * (1 - (t / duration)) + movePosition.position * (t / duration);
    }

    IEnumerator ResetPos()
    {
        yield return new WaitForSeconds(respawnDuration);
        button.move = false;
        t = 0.0f;
        Wall.transform.position = initialpos;
        button.GetComponent<Renderer>().material = button.offMat;
        teleportBack = true;
        buttonCollider.enabled = true;
    }

}

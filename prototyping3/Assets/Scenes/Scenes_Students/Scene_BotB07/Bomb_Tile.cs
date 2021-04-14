using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Tile : MonoBehaviour
{
    bool isBomb = false;
    public int BombPrecentage = 45;
    public Color FlashColor;
    public Color Default;
    public int numFlashes = 4;
    public float timeBetweenFlash = 0.05f;

    public GameObject BombCollider;
    Vector3 BombStartPos;

    float force = 50;
    bool Exited = true;

    private void OnCollisionEnter(Collision other)
    {
        if((other.transform.root.tag == "Player1" || other.transform.root.tag == "Player2") && isBomb)
        {
            Exited = false;
            StartCoroutine(FlashTillExplode(other));
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if ((other.transform.root.tag == "Player1" || other.transform.root.tag == "Player2") && isBomb)
        {
            Exited = true;
        }
    }

    IEnumerator FlashTillExplode(Collision other)
    {
       // save the InputField.textComponent color
       Color defaultColor = gameObject.GetComponent<Renderer>().material.color;
       for (int i = 0; i < numFlashes; i++)
       {
           // if the current color is the default color - change it to the flash color
           if (gameObject.GetComponent<Renderer>().material.color == Default)
           {
                gameObject.GetComponent<Renderer>().material.color = FlashColor;
           }
           else // otherwise change it back to the default color
           {
                gameObject.GetComponent<Renderer>().material.color = Default;
           }
           yield return new WaitForSeconds(timeBetweenFlash);
       }

        if (Exited == false)
        {
            //Destroy(input.gameObject, 1); // magic door closes - remove object
            GameObject CollidingPlayer = other.gameObject;
            // Calculate Angle Between the collision point and the player
            Vector3 dir = other.gameObject.transform.position - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            while (CollidingPlayer.name != CollidingPlayer.transform.root.name)
            {
                if (CollidingPlayer.transform.root.name == CollidingPlayer.transform.parent.name)
                {
                    break;
                }

                CollidingPlayer = CollidingPlayer.transform.parent.gameObject;
            }

            CollidingPlayer.GetComponent<Rigidbody>().velocity = dir * force;//AddForce(dir* force);

            //BombStartPos.y += 100;
            //BombCollider.transform.localPosition = BombStartPos;
            yield return new WaitForSeconds(Time.deltaTime);
            //BombStartPos.y -= 100;
            //BombCollider.transform.localPosition = BombStartPos;
        }

        yield return new WaitForSeconds(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        int CurrentBombPrecentage = Random.Range(0, 101);

        if (CurrentBombPrecentage <= BombPrecentage)
            isBomb = true;

        BombStartPos = BombCollider.transform.localPosition;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBotsLogic : MonoBehaviour
{
    public GameObject FallAwayCenter;
    public GameObject FallAwayIR1;
    public GameObject FallAwayIR2;
    public GameObject FallAwayIR3;
    public GameObject FallAwayIR4;
    public GameObject FallAwayIR5;
    public GameObject FallAwayIR6;
    public GameObject FallAwayIR7;
    public GameObject FallAwayIR8;
    public GameObject FallAwayOR1;
    public GameObject FallAwayOR2;
    public GameObject FallAwayOR3;
    public GameObject FallAwayOR4;
    public GameObject FallAwayOR5;
    public GameObject FallAwayOR6;
    public GameObject FallAwayOR7;
    public GameObject FallAwayOR8;
    public GameObject GameHandler;

    int TileCount = 0;

    float Timer;
    float TimerMax = 6.0f;

    // Transform of the GameObject you want to shake
    private Transform Trans;

    // Desired duration of the shake effect
    float shakeDuration = 0f;
    float shakeTime = 3.0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    float shakeMagnitude = 0.1f;

    // A measure of how quickly the shake effect should evaporate
    float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;


    //TODO: Add Wobble, Add kill barrier, add lerp fall for tiles.
    void Start()
    {
        Timer = 10;
    }

    
    void Update()
    {
        if (GameHandler.GetComponent<GameHandler>().isGameTime && !GameHandler.GetComponent<GameHandler>().GameisPaused)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0.0f)
            {
                ++TileCount;
                SelectTile();
                Timer = TimerMax;
            }

            if (shakeDuration > 0)
            {
                Trans.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

                shakeDuration -= Time.deltaTime * dampingSpeed;
            }
            else
            {
                shakeDuration = 0f;
                //transform.localPosition = initialPosition;
            }
        }
    }

    void SelectTile()
    {
        switch (TileCount)
        {
            case 1:
                TriggerShake(FallAwayOR6);
                break;
            case 2:
                TriggerShake(FallAwayOR7);
                break;
            case 3:
                TriggerShake(FallAwayOR2);
                break;
            case 4:
                TriggerShake(FallAwayOR5);
                break;
            case 5:
                TriggerShake(FallAwayOR8);
                break;
            case 6:
                TriggerShake(FallAwayOR4);
                break;
            case 7:
                TriggerShake(FallAwayOR1);
                break;
            case 8:
                TriggerShake(FallAwayOR3);
                break;
            case 9:
                TriggerShake(FallAwayIR2);
                break;
            case 10:
                TriggerShake(FallAwayIR4);
                break;
            case 11:
                TriggerShake(FallAwayIR3);
                break;
            case 12:
                TriggerShake(FallAwayIR8);
                break;
            case 13:
                TriggerShake(FallAwayIR6);
                break;
            case 14:
                TriggerShake(FallAwayIR5);
                break;
            case 15:
                TriggerShake(FallAwayIR7);
                break;
            case 16:
                TriggerShake(FallAwayIR1);
                break;
        }
    }

    IEnumerator DropTile(GameObject Tile)
    {
        yield return new WaitForSeconds(shakeTime + 0.001f);
        //Vector3 pos = new Vector3(1000, 1000, 1000);
        //Tile.transform.position = pos;
        Tile.SetActive(false);
    }

    public void TriggerShake(GameObject tile)
    {
        shakeDuration = shakeTime;
        Trans = tile.transform;
        initialPosition = Trans.localPosition;
        StartCoroutine(DropTile(tile));
    }


}

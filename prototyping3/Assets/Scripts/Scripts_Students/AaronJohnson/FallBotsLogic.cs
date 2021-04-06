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

    int TileCount = 0;

    float Timer;
    float TimerMax = 6.0f;

    //TODO: Add Wobble, Add kill barrier, add lerp fall for tiles.
    void Start()
    {
        Timer = 10;
    }

    
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0.0f)
        {
            ++TileCount;
            SelectTile();
            Timer = TimerMax;
        }
    }

    void SelectTile()
    {
        switch (TileCount)
        {
            case 1:
                DropTile(FallAwayOR6);
                break;
            case 2:
                DropTile(FallAwayOR7);
                break;
            case 3:
                DropTile(FallAwayOR2);
                break;
            case 4:
                DropTile(FallAwayOR5);
                break;
            case 5:
                DropTile(FallAwayOR8);
                break;
            case 6:
                DropTile(FallAwayOR4);
                break;
            case 7:
                DropTile(FallAwayOR1);
                break;
            case 8:
                DropTile(FallAwayOR3);
                break;
            case 9:
                DropTile(FallAwayIR2);
                break;
            case 10:
                DropTile(FallAwayIR4);
                break;
            case 11:
                DropTile(FallAwayIR3);
                break;
            case 12:
                DropTile(FallAwayIR8);
                break;
            case 13:
                DropTile(FallAwayIR6);
                break;
            case 14:
                DropTile(FallAwayIR5);
                break;
            case 15:
                DropTile(FallAwayIR7);
                break;
            case 16:
                DropTile(FallAwayIR1);
                break;
        }
    }

    void DropTile(GameObject Tile)
    {
        Vector3 pos = new Vector3(1000, 1000, 1000);
        Tile.transform.position = pos;
    }
}

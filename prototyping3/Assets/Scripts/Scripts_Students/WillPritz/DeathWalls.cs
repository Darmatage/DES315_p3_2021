using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWalls : MonoBehaviour
{
    public GameObject NorthWall;
    public GameObject EastWall;
    public GameObject SouthWall;
    public GameObject WestWall;

    public GameHandler Handler;

    public int StartCollapseTime;
    public int FinishCollapseTime;
    public float FinalSpaceSize;

    private float WallsStartPosition;
    private float CollapseSpeed;

    public float PlayerDamageCooldownTimer = 2.0f;
    private float P1DamageCooldown = 0;
    private float P2DamageCooldown = 0;

    bool startCollapse;
    // Start is called before the first frame update
    void Start()
    {
        WallsStartPosition = NorthWall.transform.position.z;
        CollapseSpeed = (NorthWall.transform.position.z * 2 - FinalSpaceSize) / (StartCollapseTime - FinishCollapseTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(!Handler.GameisPaused)
        {
            if(Handler.gameTime == StartCollapseTime)
            {
                NorthWall.SetActive(true);
                EastWall.SetActive(true);
                SouthWall.SetActive(true);
                WestWall.SetActive(true);
            }
            if (Handler.gameTime <= StartCollapseTime && Handler.gameTime >= FinishCollapseTime)
            {
                float currMoveSpeed = CollapseSpeed * Time.deltaTime;
                NorthWall.transform.Translate(new Vector3(0,0, -currMoveSpeed));
                EastWall.transform.Translate(new Vector3(-currMoveSpeed, 0, 0));
                SouthWall.transform.Translate(new Vector3(0,0, currMoveSpeed));
                WestWall.transform.Translate(new Vector3(currMoveSpeed, 0, 0));
            }
            if(Handler.Player1Holder != null && P1DamageCooldown == 0)
            {
                if(checkDamagePlayer(Handler.Player1Holder)) P1DamageCooldown = PlayerDamageCooldownTimer;
            }
            if(P1DamageCooldown > 0)
            {
                P1DamageCooldown -= Time.deltaTime;
                if (P1DamageCooldown < 0) P1DamageCooldown = 0;
            }
            if (Handler.Player2Holder != null && P2DamageCooldown == 0)
            {
                if(checkDamagePlayer(Handler.Player2Holder)) P2DamageCooldown = PlayerDamageCooldownTimer;
            }
            if (P2DamageCooldown > 0)
            {
                P2DamageCooldown -= Time.deltaTime;
                if (P2DamageCooldown < 0) P2DamageCooldown = 0;
            }
        }
    }

    void ResetWalls()
    {
        NorthWall.transform.position = new Vector3(0, 5.055f, WallsStartPosition);
        EastWall.transform.position = new Vector3(WallsStartPosition, 5.055f, 0);
        SouthWall.transform.position = new Vector3(0, 5.055f, -WallsStartPosition);
        WestWall.transform.position = new Vector3(-WallsStartPosition, 5.055f, 0);
    }

    bool checkDamagePlayer(GameObject player)
    {
        if(player.transform.position.z > NorthWall.transform.position.z ||
           player.transform.position.x > EastWall.transform.position.x  ||
           player.transform.position.z < SouthWall.transform.position.z ||
           player.transform.position.x < WestWall.transform.position.x)
        {
            return true;
        }
        return false;
    }
}

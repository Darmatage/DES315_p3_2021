using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AustinSteadWinState : MonoBehaviour
{
    public AustinStead_Button Button1;
    public AustinStead_Button Button2;

    public GameHandler gameHandler;

    private void OnEnable()
    {
        Button1.ButtonActivated += Activate;
        Button2.ButtonActivated += Activate;
    }
    private void OnDisable()
    {
        Button1.ButtonActivated -= Activate;
        Button2.ButtonActivated -= Activate;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Activate()
    {
        if(Button1.IsPressed() && Button2.IsPressed())
        {
            //Victory
            gameHandler.coopPlayersWin = true;

            StartCoroutine(gameHandler.CoopEndGame());
        }
    }

}

using UnityEngine;
using UnityEngine.Assertions;

public class botB01_Weapons : MonoBehaviour
{
    private     string[] buttons;
    private       bool[] buttonStatuses; // False if the attack IS available
    
    private botB01_PushBack       scrPushBack;
    private botB01_EarthQuake     scrEarthQuake;
    private botB01_RocketLauncher scrRocketLauncher;
    private botB01_DeployLaser    scrDeployLaser;

    private BotBasic_Move scrMove;

    void Start()
    {
        var actionScript = gameObject.transform.parent.GetComponent<playerParent>();
        buttons = new[] { actionScript.action1Input, 
                          actionScript.action2Input, 
                          actionScript.action3Input, 
                          actionScript.action4Input };
        buttonStatuses = new bool[4];
        
        scrPushBack       = GetComponentInChildren<botB01_PushBack>();
        scrEarthQuake     = GetComponentInChildren<botB01_EarthQuake>();
        scrRocketLauncher = GetComponentInChildren<botB01_RocketLauncher>();
        scrDeployLaser    = GetComponentInChildren<botB01_DeployLaser>();

        scrMove = GetComponent<BotBasic_Move>();
    }

    void Update()
    {
        if (scrMove.isGrabbed || !scrMove.isGrounded || scrMove.isTurtled)
            CancelAllAttacks();

        else
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (Input.GetButtonDown(buttons[i]) && !buttonStatuses[i])
                    Attack(i);
            }
        }
    }

    #region Helpers
    
    private void Attack(int index)
    {
        switch (index)
        {
            case 0:
                scrPushBack.Attack();
                break;
            case 1:
                scrEarthQuake.Attack();
                break;
            case 2:
                scrDeployLaser.Attack();
                break;
            case 3:
                scrRocketLauncher.Attack();
                break;
            default:
                string error = "Invalid button status index; index was " + index;
                throw new AssertionException(error, error);
        }
    }

    public void SetButtonStatus(int index, bool status)
    {
        if (index < 0 || index >= buttonStatuses.Length)
        {
            string error = "Invalid button status index; index was " + index;
            throw new AssertionException(error, error);
        }  

        buttonStatuses[index] = status;
    }

    public bool GetButtonStatus(int index)
    {
        if (index < 0 || index >= buttonStatuses.Length)
        {
            string error = "Invalid button status index; index was " + index;
            throw new AssertionException(error, error);
        }  

        return buttonStatuses[index];
    }

    public void CancelAttack(int index)
    {
        switch (index)
        {
            case 0:
                if (buttonStatuses[index])
                    scrPushBack.Cancel();
                break;
            case 1:
                if (buttonStatuses[index])
                    scrEarthQuake.Cancel();
                break;
            case 2:
                if (buttonStatuses[index])
                    scrDeployLaser.Cancel();
                break;
            case 3:
                if (buttonStatuses[index])
                    scrRocketLauncher.Cancel();
                break;
            default:
                string error = "Invalid button status index; index was " + index;
                throw new AssertionException(error, error);
        }
    }

    public void CancelAllAttacks()
    {
        for (int i = 0; i < buttons.Length; i++)
            CancelAttack(i);
    }

    #endregion
}

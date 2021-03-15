using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot09_Weapon : MonoBehaviour
{
    public GameObject weaponRake;
    public BoxCollider spikes;
    public float weaponDownTime;
    public float weaponRotateDownTime;
    public float weaponRotateUpTime;
    public AudioSource audioThing;
    public float rotation, rotatdown, rotatreset;
    public ParticleSystem wowy;
    private bool SwingDown, SwingUp;
    public WeaponRotate saws;
    public Material idleMat, attackMat;
    public MeshRenderer[] changeColorObj;

    //grab axis from parent object
    public string button1;
    public string button2;
    public string button3;
    public string button4; // currently boost in player move script

    // Start is called before the first frame update
    void Start()
    {
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;
        SwingDown = SwingUp = false;
        rotation = 0f;
        spikes.enabled = false;
        saws.rotate = false;
        foreach(MeshRenderer mesh in changeColorObj)
        {
            mesh.material = idleMat;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T)){
        if ((Input.GetButtonDown(button1)) && (SwingDown == false) && (SwingUp == false) && saws.curRotSpeed < 50.0f)
        {
            SwingDown = true;
            spikes.enabled = true;
            foreach (MeshRenderer mesh in changeColorObj)
            {
                mesh.material = attackMat;
            }
            saws.StartSaws();

        }

        if (SwingDown && rotation < rotatdown && saws.curRotSpeed > 600.0f)
        {
            weaponRake.transform.Rotate(Vector3.right * weaponRotateDownTime * Time.deltaTime);
            rotation += weaponRotateDownTime * Time.deltaTime;
            if (rotation > rotatdown)
            {
                audioThing.Play();
                wowy.Play();
                StartCoroutine(WithdrawWeapon());
            }
        }

        if(SwingUp && rotation > rotatreset)
        {
            weaponRake.transform.Rotate(-(Vector3.right * weaponRotateUpTime * Time.deltaTime));
            rotation -= weaponRotateUpTime * Time.deltaTime; 
            if (rotation < rotatreset)
            {
                // rotation = 0f;
                // transform.rotation = Quaternion.identity;
                SwingUp = false;
            }
        }


    }

    IEnumerator WithdrawWeapon()
    {
        yield return new WaitForSeconds(weaponDownTime);
        foreach (MeshRenderer mesh in changeColorObj)
        {
            mesh.material = idleMat;
        }
        SwingDown = false;
        SwingUp = true;
        spikes.enabled = false;
        saws.SlowDownSaws();
    }

}

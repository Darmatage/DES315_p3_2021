using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mars_Weapon : MonoBehaviour{
    //NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot

	public GameObject weaponThrust;
	private float thrustAmount = 3f;
    private float cooldown = 0f;
	
	private bool weaponOut = false;

    public AudioSource audioSource;

	//grab axis from parent object
	public string button1;
	public string button2;
	public string button3;
	public string button4; // currently boost in player move script

    public GameObject northPiece;
    public GameObject southPiece;
    public GameObject eastPiece;
    public GameObject westPiece;

    Vector3 nStartPos;
    Vector3 sStartPos;
    Vector3 eStartPos;
    Vector3 wStartPos;

    void Start(){
		button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
		button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
		button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
		button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

        nStartPos = northPiece.transform.localPosition;
        sStartPos = southPiece.transform.localPosition;
        eStartPos = eastPiece.transform.localPosition;
        wStartPos = westPiece.transform.localPosition;
    }



    void Update(){
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0f)
            {
                // stick robot pieces back in
                northPiece.transform.localPosition = nStartPos;
                southPiece.transform.localPosition = sStartPos;
                eastPiece.transform.localPosition = eStartPos;
                westPiece.transform.localPosition = wStartPos;
            }
        }

        //if (Input.GetKeyDown(KeyCode.T)){
        if (Input.GetButtonDown(button1) && (weaponOut == false) && (cooldown <= 0f))
        {
            audioSource.Play();

            var x = weaponThrust.transform.localScale.x * 2;
            var y = weaponThrust.transform.localScale.y;
            var z = weaponThrust.transform.localScale.z * 2;

            weaponThrust.transform.localScale = new Vector3(x, y, z);

            weaponOut = true;
            cooldown = 3f;
            StartCoroutine(WithdrawWeapon());

            // stick out robot pieces
            northPiece.transform.localPosition = new Vector3(nStartPos.x - .015f, nStartPos.y, nStartPos.z);
            Vector3.MoveTowards()
            southPiece.transform.localPosition = new Vector3(sStartPos.x + .015f, sStartPos.y, sStartPos.z);
            eastPiece.transform.localPosition = new Vector3(eStartPos.x, eStartPos.y, eStartPos.z + .015f);
            westPiece.transform.localPosition = new Vector3(wStartPos.x, wStartPos.y, wStartPos.z - .015f);
		}
    }

	IEnumerator WithdrawWeapon(){
		yield return new WaitForSeconds(0.6f);
        var x = weaponThrust.transform.localScale.x / 2;
        var y = weaponThrust.transform.localScale.y;
        var z = weaponThrust.transform.localScale.z / 2;

        weaponThrust.transform.localScale = new Vector3(x, y, z);
        weaponOut = false;
	}

    public bool IsOnCooldown()
    {
        return cooldown > 0f;
    }
}

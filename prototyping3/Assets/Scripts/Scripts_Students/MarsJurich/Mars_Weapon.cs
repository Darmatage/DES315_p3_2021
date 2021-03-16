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

    Vector3 nEndPos;
    Vector3 sEndPos;
    Vector3 eEndPos;
    Vector3 wEndPos;

    void Start(){
		button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
		button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
		button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
		button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

        nStartPos = northPiece.transform.localPosition;
        sStartPos = southPiece.transform.localPosition;
        eStartPos = eastPiece.transform.localPosition;
        wStartPos = westPiece.transform.localPosition;

        nEndPos = new Vector3(nStartPos.x - .015f, nStartPos.y, nStartPos.z);
        sEndPos = new Vector3(sStartPos.x + .015f, sStartPos.y, sStartPos.z);
        eEndPos = new Vector3(eStartPos.x, eStartPos.y, eStartPos.z + .015f);
        wEndPos = new Vector3(wStartPos.x, wStartPos.y, wStartPos.z - .015f);
    }



    void Update(){
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;

            float t;

            // move outward
            if (cooldown > 2.8f)
            {
                t = Time.deltaTime * 0.2f;

                var n = Vector3.MoveTowards(northPiece.transform.localPosition, nEndPos, t);
                var s = Vector3.MoveTowards(southPiece.transform.localPosition, sEndPos, t);
                var e = Vector3.MoveTowards(eastPiece.transform.localPosition, eEndPos, t);
                var w = Vector3.MoveTowards(westPiece.transform.localPosition, wEndPos, t);

                northPiece.transform.localPosition = n;
                southPiece.transform.localPosition = s;
                eastPiece.transform.localPosition = e;
                westPiece.transform.localPosition = w;
            }
            // move inward
            else if (cooldown > 0f)
            {
                t = Time.deltaTime / 180.8f;

                var n = Vector3.MoveTowards(northPiece.transform.localPosition, nStartPos, t);
                var s = Vector3.MoveTowards(southPiece.transform.localPosition, sStartPos, t);
                var e = Vector3.MoveTowards(eastPiece.transform.localPosition, eStartPos, t);
                var w = Vector3.MoveTowards(westPiece.transform.localPosition, wStartPos, t);

                northPiece.transform.localPosition = n;
                southPiece.transform.localPosition = s;
                eastPiece.transform.localPosition = e;
                westPiece.transform.localPosition = w;
            }
            // end of cooldown; lock back in place
            else if (cooldown <= 0f)
            {
                // stick robot pieces back in
                northPiece.transform.localPosition = nStartPos;
                southPiece.transform.localPosition = sStartPos;
                eastPiece.transform.localPosition = eStartPos;
                westPiece.transform.localPosition = wStartPos;
            }
        }
        
        if (Input.GetButtonDown(button1) && (weaponOut == false) && (cooldown <= 0f))
        {
            audioSource.Play();

            weaponThrust.transform.localScale = new Vector3(14f, 3.8f, 14f);
            weaponThrust.GetComponent<HazardDamage>().damage = 5;

            weaponOut = true;
            cooldown = 3f;
            StartCoroutine(WithdrawWeapon());
		}
    }

	IEnumerator WithdrawWeapon(){
		yield return new WaitForSeconds(0.6f);
        weaponThrust.transform.localScale = new Vector3();
        weaponThrust.GetComponent<HazardDamage>().damage = 0;
        weaponOut = false;
	}

    public bool IsOnCooldown()
    {
        return cooldown > 0f;
    }
}

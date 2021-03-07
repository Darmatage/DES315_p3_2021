using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magnet : MonoBehaviour
{
    public GameObject OtherObjectRoot;
    public GameObject otherboi;
    bool TurnOnMagnet = false;
    public GameObject GOslider;

    private void OnTriggerStay(Collider other)
    {
        if (TurnOnMagnet)
        {
            if (other.transform.root.tag == "Player1" && gameObject.transform.root.tag != "Player1" || other.transform.root.tag == "Player2" && gameObject.transform.root.tag != "Player2")
            {
                otherboi = other.gameObject;
                OtherObjectRoot = other.transform.parent.gameObject;
                other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                other.gameObject.GetComponent<BotBasic_Move>().isGrabbed = true;
                other.gameObject.GetComponent<Transform>().SetParent(transform);
            }
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (!TurnOnMagnet)
    //    {
    //        if (other.transform.root.tag == "Player1" && gameObject.transform.root.tag != "Player1" || other.transform.root.tag == "Player2" && gameObject.transform.root.tag != "Player2")
    //        {
    //            otherboi = null;
    //            OtherObjectRoot = null;
    //            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    //            other.gameObject.GetComponent<BotBasic_Move>().isGrabbed = false;
    //            other.gameObject.GetComponent<Transform>().parent = OtherObjectRoot.transform;
    //        }
    //    }
    //}

    GameObject newslider = null;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && TurnOnMagnet == false)
        {
            TurnOnMagnet = true;
        }

        if (TurnOnMagnet && otherboi)
        {
           if(!newslider)
            {
                newslider = Instantiate(GOslider);
                newslider.transform.parent = GameObject.Find("Canvas").transform;
                newslider.transform.localPosition = new Vector3(394.7f, -302.0f, 0.0f);
            }

            Slider slide =  newslider.GetComponent<Slider>();
            
            if(Input.GetKeyDown(KeyCode.RightControl))
            {
                slide.value += 4;
                
            }

            if (slide.value >= slide.maxValue)
                BrakContact();

            if (newslider)
                slide.value -= Time.deltaTime;
        }
    }

    private void BrakContact()
    {

        otherboi.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        otherboi.gameObject.GetComponent<BotBasic_Move>().isGrabbed = false;
        otherboi.transform.parent = OtherObjectRoot.transform;
        otherboi = null;
        OtherObjectRoot = null;
        Destroy(newslider);
        TurnOnMagnet = false;
    }
}

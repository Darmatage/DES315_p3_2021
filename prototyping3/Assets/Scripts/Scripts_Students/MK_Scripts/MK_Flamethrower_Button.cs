using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MK_Flamethrower_Button : MonoBehaviour
{
    public GameObject firePrefab;
    public GameObject indicator;
    public GameObject theButton;
    public GameObject source;
    private float theButtonUpPos;
    public bool active = false;

    void Start()
    {
        if (theButton != null){
            theButtonUpPos = theButton.transform.position.y;
        }
    }

    private IEnumerator TriggerActivation()
    {
        yield return new WaitForSeconds(1.0f);
        indicator.SetActive(false);

        //spawn fire objects
        for (int i = 0; i < 6; i++)
        {
            Destroy(Instantiate(firePrefab, source.transform), 3.0f);
            yield return new WaitForSeconds(.5f);
        }
        
        ButtonUp();
    }

    public void OnTriggerEnter(Collider other){
        if ((other.transform.root.gameObject.CompareTag("Player1"))||(other.transform.root.gameObject.CompareTag("Player2")) && !active)
        {
            theButton.transform.position = new Vector3(theButton.transform.position.x, theButtonUpPos - 0.4f,
                theButton.transform.position.z);
            indicator.SetActive(true);
            active = true;
            Renderer buttonRend = theButton.GetComponent<Renderer>();
            buttonRend.material.color = new Color(2.0f, 0.5f, 0.5f, 2.5f); 
            StartCoroutine(TriggerActivation());
        }
    }

    public void ButtonUp(){
        active = false;
        theButton.transform.position = new Vector3(theButton.transform.position.x, theButtonUpPos, theButton.transform.position.z);
        Renderer buttonRend = theButton.GetComponent<Renderer>();
        buttonRend.material.color = Color.white;
    }
}

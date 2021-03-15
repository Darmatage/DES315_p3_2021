using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A08_Projectile : MonoBehaviour
{

    public GameObject destroyParticle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
       
       if ((other.transform.root.tag == "Player1" && this.transform.GetChild(0).GetComponent<HazardDamage>().isPlayer2Weapon) ||
           (other.transform.root.tag == "Player2" && this.transform.GetChild(0).GetComponent<HazardDamage>().isPlayer1Weapon))
       {
            Instantiate(destroyParticle, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
       }

        
    }
}

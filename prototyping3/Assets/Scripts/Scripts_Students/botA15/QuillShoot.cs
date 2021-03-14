using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuillShoot : MonoBehaviour
{
    public GameObject quillPrefab;
    public GameObject SpawnPoint;
    public float quillSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShootQuills()
    {
        StartCoroutine(shoot());
    }

    IEnumerator shoot()
    {
        List<GameObject> quills = new List<GameObject>();

        for (int i = 0; i < 3; ++i)
        {
            GameObject quill = Instantiate(quillPrefab);
            quill.transform.position = SpawnPoint.transform.position;
            float randX = Random.Range(-1f, 1f);
            float randZ = Random.Range(-1f, 1f);
            quill.GetComponent<Rigidbody>().velocity = quillSpeed * new Vector3(-transform.forward.x + randX, transform.forward.y, -transform.forward.z + randZ);

            if (gameObject.transform.root.tag == "Player1") { quill.GetComponent<HazardDamage>().isPlayer1Weapon = true; }
            if (gameObject.transform.root.tag == "Player2") { quill.GetComponent<HazardDamage>().isPlayer2Weapon = true; }

            quills.Add(quill);
        }

        yield return new WaitForSeconds(2.0f);

        foreach(GameObject obj in quills)
        {
            Destroy(obj);
            //quills.
        }

        quills.Clear();
    }
}

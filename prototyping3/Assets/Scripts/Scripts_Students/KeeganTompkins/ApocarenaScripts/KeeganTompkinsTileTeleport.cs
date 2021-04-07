using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeganTompkinsTileTeleport : MonoBehaviour
{
    float Offset = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.root.tag == "Player1" || collision.gameObject.transform.root.tag == "Player2")
        {
            collision.gameObject.transform.position = FindObjectOfType<TileManager>().RandomTilePos() + new Vector3 (0.0f, Offset, 0.0f);
            collision.gameObject.transform.root.GetComponentInChildren<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}

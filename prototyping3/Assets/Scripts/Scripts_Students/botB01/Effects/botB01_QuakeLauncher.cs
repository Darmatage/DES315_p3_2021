using UnityEngine;

public class botB01_QuakeLauncher : MonoBehaviour
{
    public Transform source;
    [Range(0, 1000)] public float Knockback = 100.0f;
  
    private void OnTriggerEnter(Collider other)
    {
        var obj = other.transform.root;
        if (source == null || source.root.GetInstanceID() == obj.GetInstanceID() || !obj.tag.Contains("Player"))
          return;
            
        Rigidbody rb = other.transform.root.GetChild(0).GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * Knockback, ForceMode.Impulse);
    
        transform.GetComponent<Collider>().enabled = false;
    }
}

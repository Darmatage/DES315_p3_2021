using UnityEngine;

public class botB01_PushBackLauncher : MonoBehaviour
{
    public Transform source;
    [Range(0, 1000)] public float Knockback = 125;

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.transform.root;
        if (source == null || source.root.GetInstanceID() == obj.GetInstanceID() || !obj.tag.Contains("Player"))
            return;
        
        Vector3 dir = new Vector3(other.transform.position.x - source.position.x, 0, other.transform.position.z - source.position.z).normalized;
        dir += Vector3.up * 0.5f;
        
        Rigidbody rb = other.transform.root.GetChild(0).GetComponent<Rigidbody>();
        rb.AddForce(dir * Knockback, ForceMode.Impulse);

        transform.GetComponent<Collider>().enabled = false;
    }
}

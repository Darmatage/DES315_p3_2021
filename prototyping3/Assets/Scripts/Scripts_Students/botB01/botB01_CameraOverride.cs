using UnityEngine;

public class botB01_CameraOverride : MonoBehaviour
{
    [Range(-10, 10)] public float Shift;
    
    private void Start()
    {
        var cam = transform.root.GetComponentInChildren<CameraFollow>();
        if (cam != null)
        {
            cam.offsetCamera += Vector3.up * Shift;
        }
    }
}
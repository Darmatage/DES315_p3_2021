using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private float speed = 360f;
    
    // Start is called before the first frame update
    void Update()
    {
        Quaternion r = transform.localRotation;
        float newRot = r.eulerAngles.y + speed * Time.deltaTime;
        r.eulerAngles = new Vector3(0, newRot, 0);
        transform.localRotation = r;
    }

}

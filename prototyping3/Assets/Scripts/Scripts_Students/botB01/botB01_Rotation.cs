using UnityEngine;
using Random = UnityEngine.Random;

public class botB01_Rotation : MonoBehaviour
{
    [Range(0, 5)] public float Speed;
    [Range(90.0f, 3600.0f)] public float Scale;

    private BotBasic_Move movement;
    
    private void Awake()
    {
        transform.Rotate(Vector3.up, Random.value * 360.0f);
        movement = transform.parent.parent.GetComponent<BotBasic_Move>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.AngleAxis(Scale * 0.5f * (1.0f + Mathf.Sin(Time.time * Speed)), Vector3.up);
        transform.Rotate(Vector3.down, Input.GetAxisRaw(movement.pHorizontal) * movement.rotateSpeed * Time.deltaTime);
    }
}

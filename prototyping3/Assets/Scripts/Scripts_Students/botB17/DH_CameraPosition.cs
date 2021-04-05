using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_CameraPosition : MonoBehaviour
{
  public Vector3 position = new Vector3(0, 7, -7);
  public float rotation = 20f;
    // Start is called before the first frame update
  void Start()
  {
    Destroy(transform.root.GetComponentInChildren<CameraFollow>());
    DH_CameraFollow camera = transform.root.GetChild(1).gameObject.AddComponent<DH_CameraFollow>() as DH_CameraFollow;

    if (camera)
    {
      camera.offsetCamera = position;
      camera.alsoFollowRotation = true;
      camera.angleCamera = rotation;
    }
  }

  private void Update()
  {
    //CameraFollow camera = transform.root.GetComponentInChildren<CameraFollow>();
    //camera.transform.rotation = Quaternion.Euler(rotation);
  }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_FlareButton : MonoBehaviour
{
  bool pressed = false;
  float pressedCooldown = 5.0f;
  float pressedTimer = 0f;

    // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
    if(pressedTimer > 0)
    {
      pressedTimer -= Time.deltaTime;
    }
    else if(pressed)
    {
      pressed = false;
      transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y + 0.2f, transform.GetChild(0).transform.position.z);
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    PressButton();
  }

  public void PressButton()
  {
    if (!pressed)
    {
      pressed = true;
      pressedTimer = pressedCooldown;
      transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y - 0.2f, transform.GetChild(0).transform.position.z);
      transform.root.GetComponent<DH_FlareTrap>().TurnOnFlare();
    }
  }
}

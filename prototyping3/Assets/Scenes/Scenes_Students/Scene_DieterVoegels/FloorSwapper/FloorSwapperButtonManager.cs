using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwapperButtonManager : MonoBehaviour
{
  //Floor Swapper System reference
  FloorSwapperManager floorSwapperSystem;

  //Button reference
  GameObject button;
  Renderer buttonRenderer;

  //Button variables
  Vector3 defaultPosition;
  Vector3 pushedPosition;
  Color defaultColor;
  Color pushedColor;
  Color inactiveColor;
  bool active;

  private void Start()
  {
    //Get the floor swapper system script reference
    floorSwapperSystem = GetComponentInParent<FloorSwapperManager>();

    //Get the button game object refrence and the button's renderer reference
    button = transform.Find("Button").gameObject;
    buttonRenderer = button.GetComponent<Renderer>();

    //Set button positions
    defaultPosition = button.transform.position;
    pushedPosition = button.transform.position - new Vector3(0.0f, 0.4f, 0.0f);

    //Set button colors
    defaultColor = Color.green;
    pushedColor = Color.red;
    inactiveColor = Color.gray;

    //Set button to be active
    active = true;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (active == true)
    {
      //Set the position the the push position and the color to the pushed color
      button.transform.position = pushedPosition;
      buttonRenderer.material.color = pushedColor;
      buttonRenderer.material.SetColor("_EmissionColor", pushedColor);

      //Activate the floor swapper
      floorSwapperSystem.Activate();
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (active == true && buttonRenderer.material.color == pushedColor)
    {
      //Set the position to the default position and the color to the inactive color
      button.transform.position = defaultPosition;
      buttonRenderer.material.color = inactiveColor;
      buttonRenderer.material.SetColor("_EmissionColor", inactiveColor);

      //Set button to be inactive
      active = false;
    }
  }

  public void ResetButton()
  {
    //Reset position and color
    button.transform.position = defaultPosition;
    buttonRenderer.material.color = defaultColor;
    buttonRenderer.material.SetColor("_EmissionColor", defaultColor);

    //Reset button to be active
    active = true;
  }
}

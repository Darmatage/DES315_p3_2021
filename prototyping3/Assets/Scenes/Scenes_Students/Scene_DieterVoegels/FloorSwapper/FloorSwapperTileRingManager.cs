using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwapperTileRingManager : MonoBehaviour
{
  //Tile ring references
  GameObject top;
  GameObject bottom;
  GameObject right;
  GameObject left;

  //Tile ring starting position and scale
  Vector3 topStartingPosition;
  Vector3 topStartingScale;
  Vector3 bottomStartingPosition;
  Vector3 bottomStartingScale;
  Vector3 rightStartingPosition;
  Vector3 rightStartingScale;
  Vector3 leftStartingPosition;
  Vector3 leftStartingScale;

  //Tile ring ending position and scale
  Vector3 topEndingPosition;
  Vector3 topEndingScale;
  Vector3 bottomEndingPosition;
  Vector3 bottomEndingScale;
  Vector3 rightEndingPosition;
  Vector3 rightEndingScale;
  Vector3 leftEndingPosition;
  Vector3 leftEndingScale;

  void Start()
  {
    //Get tile ring references
    top = transform.Find("Top").gameObject;
    bottom = transform.Find("Bottom").gameObject;
    right = transform.Find("Right").gameObject;
    left = transform.Find("Left").gameObject;

    //get the top of the tile ring's starting position and scale, and ending position and scale
    topStartingPosition = top.transform.localPosition;
    topStartingScale = top.transform.localScale;
    topEndingPosition = new Vector3(-0.45f, 0.0f, 0.475f);
    topEndingScale = new Vector3(0.0f, 1.1f, 0.05f);

    //get the bottom of the tile ring's starting position and scale, and ending position and scale
    bottomStartingPosition = bottom.transform.localPosition;
    bottomStartingScale = bottom.transform.localScale;
    bottomEndingPosition = new Vector3(0.45f, 0.0f, -0.475f);
    bottomEndingScale = new Vector3(0.0f, 1.1f, 0.05f);

    //get the right of the tile ring's starting position and scale, and ending position and scale
    rightStartingPosition = right.transform.localPosition;
    rightStartingScale = right.transform.localScale;
    rightEndingPosition = new Vector3(0.475f, 0.0f, 0.45f);
    rightEndingScale = new Vector3(0.05f, 1.1f, 0.0f);

    //get the left of the tile ring's starting position and scale, and ending position and scale
    leftStartingPosition = left.transform.localPosition;
    leftStartingScale = left.transform.localScale;
    leftEndingPosition = new Vector3(-0.475f, 0.0f, -0.45f);
    leftEndingScale = new Vector3(0.05f, 1.1f, 0.0f);
  }

  public void UpdateRingForward(float ratio)
  {
    if (ratio > 0.75f)
    {
      top.transform.localPosition = Vector3.Lerp(topEndingPosition, topStartingPosition, (ratio - 0.75f) / 0.25f);
      top.transform.localScale = Vector3.Lerp(topEndingScale, topStartingScale, (ratio - 0.75f) / 0.25f);
    }
    else if (ratio > 0.5f)
    {
      top.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

      left.transform.localPosition = Vector3.Lerp(leftEndingPosition, leftStartingPosition, (ratio - 0.5f) / 0.25f);
      left.transform.localScale = Vector3.Lerp(leftEndingScale, leftStartingScale, (ratio - 0.5f) / 0.25f);
    }
    else if (ratio > 0.25f)
    {
      left.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

      bottom.transform.localPosition = Vector3.Lerp(bottomEndingPosition, bottomStartingPosition, (ratio - 0.25f) / 0.25f);
      bottom.transform.localScale = Vector3.Lerp(bottomEndingScale, bottomStartingScale, (ratio - 0.25f) / 0.25f);
    }
    else if (ratio > 0.0f)
    {
      bottom.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

      right.transform.localPosition = Vector3.Lerp(rightEndingPosition, rightStartingPosition, ratio / 0.25f);
      right.transform.localScale = Vector3.Lerp(rightEndingScale, rightStartingScale, ratio / 0.25f);
    }
    else
    {
      right.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }
  }

  public void UpdateRingReverse(float ratio)
  {
    if (ratio > 0.75f)
    {
      right.transform.localPosition = Vector3.Lerp(rightStartingPosition, rightEndingPosition, (ratio - 0.75f) / 0.25f);
      right.transform.localScale = Vector3.Lerp(rightStartingScale, rightEndingScale, (ratio - 0.75f) / 0.25f);
    }
    else if (ratio > 0.5f)
    {
      bottom.transform.localPosition = Vector3.Lerp(bottomStartingPosition, bottomEndingPosition, (ratio - 0.5f) / 0.25f);
      bottom.transform.localScale = Vector3.Lerp(bottomStartingScale, bottomEndingScale, (ratio - 0.5f) / 0.25f);
    }
    else if (ratio > 0.25f)
    {
      left.transform.localPosition = Vector3.Lerp(leftStartingPosition, leftEndingPosition, (ratio - 0.25f) / 0.25f);
      left.transform.localScale = Vector3.Lerp(leftStartingScale, leftEndingScale, (ratio - 0.25f) / 0.25f);
    }
    else if (ratio > 0.0f)
    {
      top.transform.localPosition = Vector3.Lerp(topStartingPosition, topEndingPosition, ratio / 0.25f);
      top.transform.localScale = Vector3.Lerp(topStartingScale, topEndingScale, ratio / 0.25f);
    }
    else
    {
      
    }
  }

  public void ResetRing()
  {
    //Reset the top portion of the ring
    top.transform.localPosition = topStartingPosition;
    top.transform.localScale = topStartingScale;

    //Reset the bottom portion of the ring
    bottom.transform.localPosition = bottomStartingPosition;
    bottom.transform.localScale = bottomStartingScale;

    //Reset the right portion of the ring
    right.transform.localPosition = rightStartingPosition;
    right.transform.localScale = rightStartingScale;

    //Reset the left portion of the ring
    left.transform.localPosition = leftStartingPosition;
    left.transform.localScale = leftStartingScale;
  }
}

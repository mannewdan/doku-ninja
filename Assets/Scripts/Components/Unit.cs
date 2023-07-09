using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
  public Point pos;

  public void Snap() {
    transform.localPosition = new Vector3(pos.x, pos.y);
  }
}

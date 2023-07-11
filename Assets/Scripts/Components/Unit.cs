using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
  public Point pos { get { return _pos; } set { _pos = value; Snap(); } }
  [SerializeField] private Point _pos;

  public void Snap() {
    transform.localPosition = new Vector3(pos.x, pos.y);
  }
}

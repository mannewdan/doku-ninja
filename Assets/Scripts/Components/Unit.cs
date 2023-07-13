using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
  public Point pos {
    get { return _pos; }
    set {
      if (value != _pos) _lastDir = (value - _pos).Normalized(true);
      _pos = value;
      Snap();
    }
  }
  [SerializeField] private Point _pos;
  public Point lastDirection { get { return _lastDir; } set { _lastDir = value; } }
  [SerializeField] private Point _lastDir = new Point(1, 0);

  public void Snap() {
    transform.localPosition = new Vector3(pos.x, pos.y);
  }
}

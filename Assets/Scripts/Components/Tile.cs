using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
  public Point pos;
  public Vector2 center { get { return new Vector3(pos.x, pos.y); } }

  void Snap() {
    transform.localPosition = center;
    transform.localScale = new Vector3(1, 1, 1);
  }
  public void Load(TileData p) {
    pos = p.pos;
    Snap();
  }
}

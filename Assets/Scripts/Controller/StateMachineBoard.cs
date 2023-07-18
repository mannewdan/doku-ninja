using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBoard : StateMachine, IPersistence {
  [SerializeField] GameObject selectionIndicatorPrefab;

  public IPersistence persistence { get { return (IPersistence)this; } }
  private Transform _marker;
  public Transform marker {
    get {
      if (_marker == null) {
        GameObject instance = Instantiate(selectionIndicatorPrefab) as GameObject;
        instance.transform.SetParent(transform);
        _marker = instance.transform;
      }
      return _marker;
    }
  }
  private Point _pos;
  public Point pos {
    get { return _pos; }
    set { _pos = value; marker.position = new Vector3(_pos.x, _pos.y); }
  }
  public Grid grid;
  public UnitManager units;
  public UnitController player { get { return units.player; } }
  public List<UnitController> enemies { get { return units.enemies; } }
  public string mapToLoad;

  public bool IsOccupied(Point p) {
    if (player.pos == p) return true;
    foreach (UnitController u in enemies) {
      if (u.pos == p) return true;
    }
    return false;
  }
  public bool InBounds(Point p) {
    if (p.x < 0 || p.x > grid.width - 1) return false;
    if (p.y < 0 || p.y > grid.height - 1) return false;
    return true;
  }
  public Point BestPos(Point start, Point dir) {
    var newPos = start + dir;
    //desired pos
    if (InBounds(newPos)) return newPos;

    //adjacent pos
    var adjacent = new Point(dir.y, dir.x);
    newPos = start + adjacent;
    if (InBounds(newPos)) return newPos;

    newPos = start - adjacent;
    if (InBounds(newPos)) return newPos;

    //opposite pos
    newPos = start - dir;
    if (InBounds(newPos)) return newPos;

    return start;
  }
}

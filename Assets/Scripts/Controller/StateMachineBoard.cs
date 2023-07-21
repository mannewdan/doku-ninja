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

  public Point BestPos(Point start, Point dir) {
    var newPos = start + dir;
    //desired pos
    if (grid.InBounds(newPos)) return newPos;

    //adjacent pos
    var adjacent = new Point(dir.y, dir.x);
    newPos = start + adjacent;
    if (grid.InBounds(newPos)) return newPos;

    newPos = start - adjacent;
    if (grid.InBounds(newPos)) return newPos;

    //opposite pos
    newPos = start - dir;
    if (grid.InBounds(newPos)) return newPos;

    return start;
  }
}

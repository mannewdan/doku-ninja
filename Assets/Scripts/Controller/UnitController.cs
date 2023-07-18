using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {
  public Point pos {
    get { return _pos; }
    set {
      if (value != _pos) {
        _lastDir = (value - _pos).Normalized(true);
        _pos = value;
        gameObject.PostNotification(Notifications.UNIT_MOVED);
      }
    }
  }
  public Point lastDirection { get { return _lastDir; } set { _lastDir = value; } }
  public UnitManager units { get { if (!_units) _units = GetComponentInParent<UnitManager>(); return _units; } }

  private UnitManager _units;
  public List<Point> targetedTiles = new List<Point>();
  [SerializeField] private Point _pos;
  [SerializeField] private Point _lastDir = new Point(1, 0);

  public void QueueAttack() {
    targetedTiles.Clear();

    var diff = units.player.pos - pos;
    if (Mathf.Abs(diff.x) + Mathf.Abs(diff.y) <= 1) {
      targetedTiles.Add(units.player.pos);
      gameObject.PostNotification(Notifications.UNIT_TELEGRAPHED, targetedTiles);
    }
  }
}

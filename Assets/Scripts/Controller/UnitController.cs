using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {
  public new UnitRenderer renderer {
    get { if (!_renderer) _renderer = GetComponent<UnitRenderer>(); return _renderer; }
  }
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
  public bool isTelegraphing { get { return targetedTiles.Count > 0; } }

  private UnitManager _units;
  private UnitRenderer _renderer;
  public List<Point> targetedTiles = new List<Point>();
  [SerializeField] private Point _pos;
  [SerializeField] private Point _lastDir = new Point(1, 0);

  public void ExecuteAttack() {
    gameObject.PostNotification(Notifications.UNIT_ATTACKED, targetedTiles);
    targetedTiles.Clear();
  }
  public void QueueAttack() {
    targetedTiles.Clear();

    var diff = units.player.pos - pos;
    if (Mathf.Abs(diff.x) + Mathf.Abs(diff.y) <= 1) {
      targetedTiles.Add(units.player.pos);
      gameObject.PostNotification(Notifications.UNIT_TELEGRAPHED, targetedTiles);
    }
  }
}

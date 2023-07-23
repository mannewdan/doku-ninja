using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class UnitController : MonoBehaviour {
  const int PH_MOVES_PER_ROUND = 1; //pull this from some UnitData class eventually

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
  public bool isActive {
    get { return _isActive; }
    set {
      if (_isActive != value) {
        _isActive = value;
        gameObject.PostNotification(Notifications.UNIT_ACTIVE_CHANGED, value);
      }
    }
  }
  public Point lastDirection { get { return _lastDir; } set { _lastDir = value; } }
  public bool isTelegraphing { get { return targetedTiles.Count > 0; } }
  public Point playerPos { get { return units.player.pos; } }

  private UnitRenderer _renderer;
  [SerializeField] private Point _pos;
  [SerializeField] private Point _lastDir = new Point(1, 0);
  [SerializeField] private bool _isActive;
  public List<Point> targetedTiles = new List<Point>();
  public Grid grid;
  public UnitManager units;
  public Pathfinder pathfinder;

  public IEnumerator<float> _ExecuteAttack() {
    gameObject.PostNotification(Notifications.UNIT_ATTACKED, targetedTiles);
    targetedTiles.Clear();
    yield break;
  }
  public IEnumerator<float> _QueueAttack() {
    targetedTiles.Clear();
    targetedTiles.Add(playerPos);
    gameObject.PostNotification(Notifications.UNIT_TELEGRAPHED, targetedTiles);
    yield break;
  }
  public IEnumerator<float> _MoveToPlayer() {
    if (pathfinder == null || !pathfinder.initialized) pathfinder = new Pathfinder(grid, units);
    pathfinder.FindPath(pos, playerPos);

    for (int i = 0; i < PH_MOVES_PER_ROUND; i++) {
      yield return Timing.WaitForSeconds(0.25f);
      if (InRange(playerPos)) {
        break;
      } else {
        if (pathfinder.NextStep(out Point p) && !units.IsOccupied(p)) {
          pos = p;
        } else {
          pos = pathfinder.SmartStep(pos, playerPos);
        }
      }
    }

    yield return Timing.WaitForSeconds(0.25f);
    if (InRange(playerPos)) yield return Timing.WaitUntilDone(Timing.RunCoroutine(_QueueAttack()));
  }
  public bool InRange(Point target) {
    var diff = target - pos;
    return Mathf.Abs(diff.x) + Mathf.Abs(diff.y) <= 1;
  }
}

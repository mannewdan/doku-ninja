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
  public Point playerPos { get { return units.player.pos; } }
  public bool isAlive { get { return _hp > 0; } }
  public int hp { get { return _hp; } }

  private UnitRenderer _renderer;
  [SerializeField] private Point _pos;
  [SerializeField] private Point _lastDir = new Point(1, 0);
  [SerializeField] private bool _isActive;
  [SerializeField] private int _hp = 2;
  public List<Point> targetedTiles = new List<Point>();
  public Grid grid;
  public UnitManager units;
  public Pathfinder pathfinder;
  public bool isPlayer;

  //commands
  public IEnumerator<float> _ExecuteAttack() {
    gameObject.PostNotification(Notifications.UNIT_ATTACKED, targetedTiles);
    foreach (Point p in targetedTiles) {
      if (p == playerPos) {
        units.player.Harm(10);
        gameObject.PostNotification(Notifications.PLAYER_HARMED, this);
      }
    }
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

    yield break;
  }
  public void TargetTiles() {
    targetedTiles.Clear();
    Point n = new Point(pos.x, pos.y + 1), e = new Point(pos.x + 1, pos.y), s = new Point(pos.x, pos.y - 1), w = new Point(pos.x - 1, pos.y);
    if (grid.InBounds(n)) targetedTiles.Add(n);
    if (grid.InBounds(e)) targetedTiles.Add(e);
    if (grid.InBounds(s)) targetedTiles.Add(s);
    if (grid.InBounds(w)) targetedTiles.Add(w);
    gameObject.PostNotification(Notifications.UNIT_TELEGRAPHED, targetedTiles);
  }
  public void ClearAttack() {
    gameObject.PostNotification(Notifications.UNIT_CANCELED, targetedTiles);
    targetedTiles.Clear();
  }
  public void Harm(int amount) {
    if (amount <= 0) return;

    _hp -= amount;
    this.PostNotification(Notifications.UNIT_DAMAGED, amount);

    if (_hp <= 0) {
      _hp = 0;
      gameObject.PostNotification(Notifications.UNIT_CANCELED, targetedTiles);
      gameObject.PostNotification(Notifications.UNIT_DESTROYED, this);
    }
  }

  //queries
  public bool InRange(Point target) {
    var diff = target - pos;
    return Mathf.Abs(diff.x) + Mathf.Abs(diff.y) <= 1;
  }
}

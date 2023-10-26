using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageState : State {
  protected StageController owner;
  public Transform marker { get { return owner.marker; } }
  public virtual Point pos { get { return owner.pos; } set { owner.pos = value; } }
  public Grid grid { get { return owner.grid; } }
  public UnitManager units { get { return owner.units; } }
  public UnitController player { get { return owner.player; } }
  public List<UnitController> enemies { get { return owner.enemies; } }
  public ActionPointsManager apManager { get { return owner.apManager; } }
  public DeckController deck { get { return owner.deck; } }

  protected virtual void Awake() {
    owner = GetComponent<StageController>();
  }
  protected override void AddObservers() {
    this.AddObserver(OnMove, Notifications.MOVE);
    this.AddObserver(OnSpentAP, Notifications.PLAYER_SPENT_AP);
    this.AddObserver(OnNumber, Notifications.NUMBER);
    this.AddObserver(OnConfirm, Notifications.CONFIRM);
    this.AddObserver(OnCancel, Notifications.CANCEL);
    this.AddObserver(OnDiscard, Notifications.DISCARD);
  }
  protected override void RemoveObservers() {
    this.RemoveObserver(OnMove, Notifications.MOVE);
    this.RemoveObserver(OnSpentAP, Notifications.PLAYER_SPENT_AP);
    this.RemoveObserver(OnNumber, Notifications.NUMBER);
    this.RemoveObserver(OnConfirm, Notifications.CONFIRM);
    this.RemoveObserver(OnCancel, Notifications.CANCEL);
    this.RemoveObserver(OnDiscard, Notifications.DISCARD);
  }

  protected virtual void OnMove(object sender, object e) { }
  protected virtual void OnSpentAP(object sender, object e) { }
  protected virtual void OnNumber(object sender, object e) { }
  protected virtual void OnConfirm(object sender, object e) { }
  protected virtual void OnCancel(object sender, object e) { }
  protected virtual void OnDiscard(object sender, object e) { }

  protected bool InBounds(Point p) { return owner.grid.InBounds(p); }
  protected bool IsOccupied(Point p) { return owner.units.IsOccupied(p); }
  protected bool IsWalkable(Point p) {
    var tile = owner.grid.tiles.ContainsKey(p) ? owner.grid.tiles[p] : null;
    return tile && tile.IsWalkable();
  }
  protected Point BestPos(Point start, Point dir, List<Point> validPoints) {
    if (dir.x == 0 && dir.y == 0) dir = new Point(1, 0);
    var newPos = start + dir;

    //best is the first tile in the same row/column in the target direction
    int failsafe = 0;
    while (grid.InBounds(newPos)) {
      if (validPoints.Contains(newPos)) return newPos;
      newPos += dir;
      failsafe++; if (failsafe > 1000) {
        Debug.Log("A loop continued for longer than expected");
        break;
      }
    }

    //next best is the closest tile in the target direction
    int closestDistance = int.MaxValue;
    foreach (Point p in validPoints) {
      if (dir.x > 0 && p.x <= start.x) continue;
      if (dir.y > 0 && p.y <= start.y) continue;
      if (dir.x < 0 && p.x >= start.x) continue;
      if (dir.y < 0 && p.y >= start.y) continue;
      var dist = Mathf.Abs(start.x - p.x) + Mathf.Abs(start.y - p.y);
      if (dist < closestDistance) {
        closestDistance = dist;
        newPos = p;
      }
    }
    if (closestDistance < int.MaxValue) return newPos;

    //last best is the closest tile in the opposite direction, but only if there are no tiles in the target direction
    closestDistance = int.MaxValue;
    foreach (Point p in validPoints) {
      if (dir.x > 0 && p.x > start.x) { closestDistance = int.MaxValue; break; };
      if (dir.y > 0 && p.y > start.y) { closestDistance = int.MaxValue; break; };
      if (dir.x < 0 && p.x < start.x) { closestDistance = int.MaxValue; break; };
      if (dir.y < 0 && p.y < start.y) { closestDistance = int.MaxValue; break; };
      var dist = Mathf.Abs(start.x - p.x) + Mathf.Abs(start.y - p.y);
      if (dist < closestDistance) {
        closestDistance = dist;
        newPos = p;
      }
    }
    if (closestDistance < int.MaxValue) return newPos;

    return start;
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageState : State {
  protected StageController owner;
  public Transform marker { get { return owner.marker; } }
  public Point pos { get { return owner.pos; } set { owner.pos = value; } }
  public Grid grid { get { return owner.grid; } }
  public UnitManager units { get { return owner.units; } }
  public UnitController player { get { return owner.player; } }
  public List<UnitController> enemies { get { return owner.enemies; } }
  public ActionPointsManager apManager { get { return owner.apManager; } }

  protected virtual void Awake() {
    owner = GetComponent<StageController>();
  }
  protected override void AddListeners() {
    this.AddObserver(OnMove, Notifications.MOVE);
    this.AddObserver(OnSpentAP, Notifications.PLAYER_SPENT_AP);
    this.AddObserver(OnNumber, Notifications.NUMBER);
    this.AddObserver(OnConfirm, Notifications.CONFIRM);
    this.AddObserver(OnCancel, Notifications.CANCEL);
  }
  protected override void RemoveListeners() {
    this.RemoveObserver(OnMove, Notifications.MOVE);
    this.RemoveObserver(OnSpentAP, Notifications.PLAYER_SPENT_AP);
    this.RemoveObserver(OnNumber, Notifications.NUMBER);
    this.RemoveObserver(OnConfirm, Notifications.CONFIRM);
    this.RemoveObserver(OnCancel, Notifications.CANCEL);
  }

  protected virtual void OnMove(object sender, object e) { }
  protected virtual void OnSpentAP(object sender, object e) { }
  protected virtual void OnNumber(object sender, object e) { }
  protected virtual void OnConfirm(object sender, object e) { }
  protected virtual void OnCancel(object sender, object e) { }

  protected bool InBounds(Point p) { return owner.grid.InBounds(p); }
  protected bool IsOccupied(Point p) { return owner.units.IsOccupied(p); }
  protected Point BestPos(Point start, Point dir) { return owner.BestPos(start, dir); }
}

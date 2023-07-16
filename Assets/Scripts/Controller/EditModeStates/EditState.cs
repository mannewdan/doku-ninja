using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditState : State {
  protected EditController owner;

  public Transform marker { get { return owner.marker; } }
  public Point pos { get { return owner.pos; } set { owner.pos = value; } }
  public Grid grid { get { return owner.grid; } }
  public UnitManager units { get { return owner.units; } }
  public MapData mapData { get { return owner.mapData; } }

  protected virtual void Awake() {
    owner = GetComponent<EditController>();
  }

  protected override void AddListeners() {
    this.AddObserver(OnMove, Notifications.MOVE);
    this.AddObserver(OnMoveRepeat, Notifications.MOVE_REPEAT);
    this.AddObserver(OnNumber, Notifications.NUMBER);
    this.AddObserver(OnTab, Notifications.TAB);
    this.AddObserver(OnConfirm, Notifications.CONFIRM);
    this.AddObserver(OnCancel, Notifications.CANCEL);
  }
  protected override void RemoveListeners() {
    this.RemoveObserver(OnMove, Notifications.MOVE);
    this.RemoveObserver(OnMoveRepeat, Notifications.MOVE_REPEAT);
    this.RemoveObserver(OnNumber, Notifications.NUMBER);
    this.RemoveObserver(OnTab, Notifications.TAB);
    this.RemoveObserver(OnConfirm, Notifications.CONFIRM);
    this.RemoveObserver(OnCancel, Notifications.CANCEL);
  }

  protected virtual void OnMove(object sender, object e) { }
  protected virtual void OnMoveRepeat(object sender, object e) { }
  protected virtual void OnNumber(object sender, object e) { }
  protected virtual void OnTab(object sender, object e) { }
  protected virtual void OnConfirm(object sender, object e) { }
  protected virtual void OnCancel(object sender, object e) { }

  protected bool InBounds(Point p) { return owner.InBounds(p); }
}

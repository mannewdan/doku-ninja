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

  protected override void AddInputObservers() {
    this.AddObserver(OnMove, Notifications.MOVE);
    this.AddObserver(OnMoveRepeat, Notifications.MOVE_REPEAT);
    this.AddObserver(OnNumber, Notifications.NUMBER);
    this.AddObserver(OnNumberModified, Notifications.CONTROL_NUMBER);
    this.AddObserver(OnTab, Notifications.TAB);
    this.AddObserver(OnConfirm, Notifications.CONFIRM);
    this.AddObserver(OnCancel, Notifications.CANCEL);
    this.AddObserver(OnSave, Notifications.SAVE);
  }
  protected override void RemoveInputObservers() {
    this.RemoveObserver(OnMove, Notifications.MOVE);
    this.RemoveObserver(OnMoveRepeat, Notifications.MOVE_REPEAT);
    this.RemoveObserver(OnNumber, Notifications.NUMBER);
    this.RemoveObserver(OnNumberModified, Notifications.CONTROL_NUMBER);
    this.RemoveObserver(OnTab, Notifications.TAB);
    this.RemoveObserver(OnConfirm, Notifications.CONFIRM);
    this.RemoveObserver(OnCancel, Notifications.CANCEL);
    this.RemoveObserver(OnSave, Notifications.SAVE);
  }

  protected virtual void OnMove(object sender, object e) { }
  protected virtual void OnMoveRepeat(object sender, object e) { }
  protected virtual void OnNumber(object sender, object e) { }
  protected virtual void OnNumberModified(object sender, object e) { }
  protected virtual void OnTab(object sender, object e) { }
  protected virtual void OnConfirm(object sender, object e) { }
  protected virtual void OnCancel(object sender, object e) { }
  protected virtual void OnSave(object sender, object e) { }

  protected bool InBounds(Point p) { return owner.grid.InBounds(p); }
}

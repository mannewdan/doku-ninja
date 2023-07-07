using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditState : State {
  protected EditController owner;

  public Transform marker { get { return owner.marker; } }
  public Point pos { get { return owner.pos; } set { owner.pos = value; } }
  public Grid grid { get { return owner.grid; } }
  public GridData gridData { get { return owner.gridData; } }

  protected virtual void Awake() {
    owner = GetComponent<EditController>();
  }
  public override void Enter() {
    base.Enter();
  }

  protected override void AddListeners() {
    this.AddObserver(OnMove, Notifications.MOVE);
    this.AddObserver(OnMoveRepeat, Notifications.MOVE_REPEAT);
    this.AddObserver(OnNumber, Notifications.NUMBER);
    this.AddObserver(OnTab, Notifications.TAB);
  }
  protected override void RemoveListeners() {
    this.RemoveObserver(OnMove, Notifications.MOVE);
    this.RemoveObserver(OnMoveRepeat, Notifications.MOVE_REPEAT);
    this.RemoveObserver(OnNumber, Notifications.NUMBER);
    this.RemoveObserver(OnTab, Notifications.TAB);
  }

  protected virtual void OnMove(object sender, object e) { }
  protected virtual void OnMoveRepeat(object sender, object e) { }
  protected virtual void OnNumber(object sender, object e) { }
  protected virtual void OnTab(object sender, object e) { }

  protected virtual void SnapMarker() {
    marker.transform.localPosition = new Vector3(pos.x, pos.y);
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditState : State {
  protected EditController owner;

  public Transform marker { get { return owner.marker; } }
  public Point pos { get { return owner.pos; } set { owner.pos = value; } }
  public Grid grid { get { return owner.grid; } }
  public GridData gridData { get { return owner.gridData; } set { owner.gridData = value; } }

  protected virtual void Awake() {
    owner = GetComponent<EditController>();
  }

  protected override void AddListeners() {
    this.AddObserver(OnMove, Notifications.MOVE);
    this.AddObserver(OnNumber, Notifications.NUMBER);
  }
  protected override void RemoveListeners() {
    this.RemoveObserver(OnMove, Notifications.MOVE);
    this.RemoveObserver(OnNumber, Notifications.NUMBER);
  }

  protected virtual void OnMove(object sender, object e) {

  }
  protected virtual void OnNumber(object sender, object e) {

  }

  protected virtual void SnapMarker() {
    marker.transform.localPosition = new Vector3(pos.x, pos.y);
  }
}

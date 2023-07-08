using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
  public UnitData data;

  public bool isPlayer { get { return false; } }
  public Point pos { get { return data.pos; } set { data.pos = value; } }

  void Awake() {
    if (data == null) data = new UnitData(false);
  }
  void Load(UnitData data) {
    this.data = data;
    if (enabled) OnEnable();
  }
  void OnEnable() {
    if (isPlayer) this.AddObserver(OnMoveEvent, Notifications.MOVE);
  }
  void OnDisable() {
    if (isPlayer) this.RemoveObserver(OnMoveEvent, Notifications.MOVE);
  }
  private void OnMoveEvent(object sender, object e) {
    pos += ((InfoEventArgs<Point>)e).info;
    Snap();
  }
  private void Snap() {
    transform.localPosition = new Vector3(pos.x, pos.y);
  }
}

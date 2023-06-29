using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
  [SerializeField] bool isPlayer;

  Point pos;

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

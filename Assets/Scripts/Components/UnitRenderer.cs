using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRenderer : MonoBehaviour {
  UnitController controller {
    get { if (!_controller) _controller = GetComponent<UnitController>(); return _controller; }
  }
  UnitController _controller;
  Point pos { get { return controller.pos; } }
  List<Point> targetedTiles { get { return controller.targetedTiles; } }
  public Telegraphs telegraphs { get { if (!_telegraphs) _telegraphs = transform.parent.parent.GetComponentInChildren<Telegraphs>(); return _telegraphs; } }

  private Telegraphs _telegraphs;

  void OnEnable() { AddObservers(); }
  void OnDisable() { RemoveObservers(); }
  void OnDestroy() { RemoveObservers(); }
  void AddObservers() {
    this.AddObserver(Snap, Notifications.UNIT_MOVED, gameObject);
    this.AddObserver(Telegraph, Notifications.UNIT_TELEGRAPHED, gameObject);
  }
  void RemoveObservers() {
    this.RemoveObserver(Snap, Notifications.UNIT_MOVED, gameObject);
    this.RemoveObserver(Telegraph, Notifications.UNIT_TELEGRAPHED, gameObject);
  }

  public void Snap(object sender, object e) {
    transform.localPosition = new Vector3(pos.x, pos.y);
  }
  public void Telegraph(object sender, object e) {
    if (e is List<Point> points) {
      foreach (Point p in points) {
        telegraphs.Add(p);
      }
    }
  }
}

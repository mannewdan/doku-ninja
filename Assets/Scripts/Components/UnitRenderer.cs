using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class UnitRenderer : MonoBehaviour {
  public UnitController controller {
    get { if (!_controller) _controller = GetComponent<UnitController>(); return _controller; }
  }
  public Point pos { get { return controller.pos; } }
  public Telegraphs telegraphs { get { if (!_telegraphs) _telegraphs = transform.parent.parent.GetComponentInChildren<Telegraphs>(); return _telegraphs; } }

  private UnitController _controller;
  private Telegraphs _telegraphs;

  void OnEnable() { AddObservers(); }
  void OnDisable() { RemoveObservers(); }
  void OnDestroy() { RemoveObservers(); }
  void AddObservers() {
    this.AddObserver(Snap, Notifications.UNIT_MOVED, gameObject);
    this.AddObserver(Telegraph, Notifications.UNIT_TELEGRAPHED, gameObject);
    this.AddObserver(Attack, Notifications.UNIT_ATTACKED, gameObject);
  }
  void RemoveObservers() {
    this.RemoveObserver(Snap, Notifications.UNIT_MOVED, gameObject);
    this.RemoveObserver(Telegraph, Notifications.UNIT_TELEGRAPHED, gameObject);
    this.RemoveObserver(Attack, Notifications.UNIT_ATTACKED, gameObject);
  }

  public void Snap(object sender, object e) {
    gameObject.TweenPosition(new Vector3(pos.x, pos.y), 0.15f).SetEaseCubicOut();
  }
  public void Telegraph(object sender, object e) {
    if (e is List<Point> points) {
      foreach (Point p in points) {
        telegraphs.Add(p);
      }
    }
  }
  public void Attack(object sender, object e) {
    if (e is List<Point> points) {
      foreach (Point p in points) {
        telegraphs.Remove(p);
      }
    }
  }
}

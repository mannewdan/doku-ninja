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

  private Transform _marker;
  public Transform marker {
    get {
      if (_marker == null) {
        GameObject instance = Instantiate(controller.units.activeIndicatorPrefab) as GameObject;
        instance.transform.SetParent(transform);
        instance.transform.localPosition = new Vector3(0, 0, 0.1f);
        _marker = instance.transform;
      }
      return _marker;
    }
  }

  void OnEnable() { AddObservers(); }
  void OnDisable() { RemoveObservers(); }
  void OnDestroy() { RemoveObservers(); }
  void AddObservers() {
    this.AddObserver(SmoothMove, Notifications.UNIT_MOVED, gameObject);
    this.AddObserver(Telegraph, Notifications.UNIT_TELEGRAPHED, gameObject);
    this.AddObserver(Attack, Notifications.UNIT_ATTACKED, gameObject);
    this.AddObserver(UpdateActiveIndicator, Notifications.UNIT_ACTIVE_CHANGED, gameObject);
  }
  void RemoveObservers() {
    this.RemoveObserver(SmoothMove, Notifications.UNIT_MOVED, gameObject);
    this.RemoveObserver(Telegraph, Notifications.UNIT_TELEGRAPHED, gameObject);
    this.RemoveObserver(Attack, Notifications.UNIT_ATTACKED, gameObject);
    this.RemoveObserver(UpdateActiveIndicator, Notifications.UNIT_ACTIVE_CHANGED, gameObject);
  }

  public void Snap() {
    transform.localPosition = new Vector3(pos.x, pos.y);
  }
  private void SmoothMove(object sender, object e) {
    var handle = gameObject.TweenPosition(new Vector3(pos.x, pos.y), 0.15f).SetEaseCubicOut();
  }
  private void Telegraph(object sender, object e) {
    if (e is List<Point> points) {
      foreach (Point p in points) {
        telegraphs.Add(p);
      }
    }
  }
  private void Attack(object sender, object e) {
    if (e is List<Point> points) {
      foreach (Point p in points) {
        telegraphs.Remove(p);
      }
    }
  }
  private void UpdateActiveIndicator(object sender, object e) {
    if (e is bool value) {
      marker.gameObject.SetActive(value);
    }
  }
}

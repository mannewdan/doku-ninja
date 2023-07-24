using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class UnitRenderer : MonoBehaviour {
  public UnitController controller {
    get { if (!_controller) _controller = GetComponent<UnitController>(); return _controller; }
  }
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
  public Point pos { get { return controller.pos; } }

  private UnitController _controller;
  private Transform _marker;
  public Telegraphs telegraphs;

  void OnEnable() { AddObservers(); }
  void OnDisable() { RemoveObservers(); }
  void OnDestroy() { RemoveObservers(); }
  void AddObservers() {
    this.AddObserver(SmoothMove, Notifications.UNIT_MOVED, gameObject);
    this.AddObserver(AddTelegraphs, Notifications.UNIT_TELEGRAPHED, gameObject);
    this.AddObserver(RemoveTelegraphs, Notifications.UNIT_ATTACKED, gameObject);
    this.AddObserver(RemoveTelegraphs, Notifications.UNIT_CANCELED, gameObject);
    this.AddObserver(UpdateActiveIndicator, Notifications.UNIT_ACTIVE_CHANGED, gameObject);
    this.AddObserver(BeDestroyed, Notifications.UNIT_DESTROYED, gameObject);
  }
  void RemoveObservers() {
    this.RemoveObserver(SmoothMove, Notifications.UNIT_MOVED, gameObject);
    this.RemoveObserver(AddTelegraphs, Notifications.UNIT_TELEGRAPHED, gameObject);
    this.RemoveObserver(RemoveTelegraphs, Notifications.UNIT_ATTACKED, gameObject);
    this.RemoveObserver(RemoveTelegraphs, Notifications.UNIT_CANCELED, gameObject);
    this.RemoveObserver(UpdateActiveIndicator, Notifications.UNIT_ACTIVE_CHANGED, gameObject);
    this.RemoveObserver(BeDestroyed, Notifications.UNIT_DESTROYED, gameObject);
  }

  public void Snap() {
    transform.localPosition = new Vector3(pos.x, pos.y);
  }
  private void SmoothMove(object sender, object e) {
    var handle = gameObject.TweenPosition(new Vector3(pos.x, pos.y), 0.15f).SetEaseCubicOut();
  }
  private void AddTelegraphs(object sender, object e) {
    if (e is List<Point> points) {
      foreach (Point p in points) {
        telegraphs.Add(p);
      }
    }
  }
  private void RemoveTelegraphs(object sender, object e) {
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
  private void BeDestroyed(object sender, object e) {
    gameObject.TweenLocalScale(Vector3.zero, 0.35f).SetEaseCubicOut().SetOnComplete(() => {
      gameObject.SetActive(false);
      if (!controller.isPlayer) {
        Destroy(gameObject);
      }
    });
  }
}

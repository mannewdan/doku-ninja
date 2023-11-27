using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;
using System.Linq;

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
  private Material _material;

  void OnEnable() {
    this.AddObserver(SmoothMove, Notifications.UNIT_MOVED, gameObject);
    this.AddObserver(UpdateActiveIndicator, Notifications.UNIT_ACTIVE_CHANGED, gameObject);
    this.AddObserver(BeDestroyed, Notifications.UNIT_DESTROYED, gameObject);
    this.AddObserver(DarkenHighlight, Notifications.SHIFT_HELD);
    this.AddObserver(ResetHighlight, Notifications.SHIFT_RELEASED);
  }
  void OnDisable() {
    this.RemoveObserver(SmoothMove, Notifications.UNIT_MOVED, gameObject);
    this.RemoveObserver(UpdateActiveIndicator, Notifications.UNIT_ACTIVE_CHANGED, gameObject);
    this.RemoveObserver(BeDestroyed, Notifications.UNIT_DESTROYED, gameObject);
    this.RemoveObserver(DarkenHighlight, Notifications.SHIFT_HELD);
    this.RemoveObserver(ResetHighlight, Notifications.SHIFT_RELEASED);
  }

  void Awake() {
    var meshRenderer = GetComponentsInChildren<MeshRenderer>().First((m) => { return m.name == "Sprite"; });
    _material = meshRenderer?.materials[0];
  }
  public void Snap() {
    transform.localPosition = new Vector3(pos.x, pos.y);
  }
  private void SmoothMove(object sender, object e) {
    gameObject.TweenPosition(new Vector3(pos.x, pos.y), 0.15f).SetEaseCubicOut();
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
  private void ResetHighlight(object sender, object e) {
    if (_material) {
      _material.SetFloat("_HighlightPower", 0);
    }
  }
  private void DarkenHighlight(object sender, object e) {
    if (_material) {
      _material.SetFloat("_HighlightPower", 0.65f);
    }
  }
}

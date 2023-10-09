using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;
using UnityEngine.UI;

public class PointIndicator : MonoBehaviour {
  public const float ACTION_POINT_SPACING = 0.15f;

  [SerializeField] private Color indicatorActiveColor;
  [SerializeField] private Color indicatorInactiveColor;

  private List<PointIndicator> indicators { get { return manager.indicators; } }
  private int index { get { return indicators.IndexOf(this); } }
  private ActionPointsManager manager;
  private Image _image;

  void Awake() {
    _image = GetComponent<Image>();
  }
  void OnEnable() {
    this.AddObserver(UpdateColor, Notifications.PLAYER_SPENT_AP);
    this.AddObserver(UpdateColor, Notifications.PLAYER_GAINED_AP);
    this.AddObserver(UpdatePosition, Notifications.PLAYER_CHANGED_AP_MAX);
  }
  void OnDisable() {
    this.RemoveObserver(UpdateColor, Notifications.PLAYER_SPENT_AP);
    this.RemoveObserver(UpdateColor, Notifications.PLAYER_GAINED_AP);
    this.RemoveObserver(UpdatePosition, Notifications.PLAYER_CHANGED_AP_MAX);
  }

  public void Initialize(ActionPointsManager manager) {
    this.manager = manager;
    transform.localPosition = GetDesiredPosition();
    transform.localScale = Vector3.zero;
    gameObject.TweenLocalScale(Vector3.one, 0.25f).SetEaseCubicOut();
    UpdateColor(this, manager.ap);
  }
  void UpdateColor(object sender, object e) {
    if (e is int ap) {
      _image.color = index >= ap ? indicatorInactiveColor : indicatorActiveColor;
    }
  }
  void UpdatePosition(object sender, object e) {
    gameObject.TweenLocalPosition(GetDesiredPosition(), 0.25f).SetEaseCubicOut();
  }
  Vector3 GetDesiredPosition() {
    var c = indicators.Count;
    var rect = GetComponent<RectTransform>();
    var width = rect ? rect.rect.width : 0;
    var left = width * 0.5f - (c * width * 0.5f) - ((c - 1) * width * ACTION_POINT_SPACING * 0.5f);
    return new Vector3(left + index * width * (1.0f + ACTION_POINT_SPACING), 0, 0);
  }
}

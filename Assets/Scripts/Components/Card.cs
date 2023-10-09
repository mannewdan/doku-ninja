using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Tweens;

public class Card : MonoBehaviour {
  const float HAND_SPACING = 0.1f; //as a percentage of card width

  public CardData data {
    get { return _data; }
    set {
      _data = value;
      digit.text = _data.value.ToString();
    }
  }
  public DeckController deck;
  public TextMeshProUGUI digit;

  private CardData _data;
  private RectTransform _rect;

  void Awake() {
    _rect = transform as RectTransform;
  }
  void OnEnable() {
    this.AddObserver(UpdatePosition, Notifications.CARD_DRAW);
    this.AddObserver(UpdatePosition, Notifications.CARD_DISCARD);
  }
  void OnDisable() {
    this.RemoveObserver(UpdatePosition, Notifications.CARD_DRAW);
    this.RemoveObserver(UpdatePosition, Notifications.CARD_DISCARD);
  }

  void UpdatePosition(object sender, object e) {
    if (deck.hand.Contains(this)) {
      var i = deck.hand.IndexOf(this);
      var c = deck.hand.Count;
      var left = _rect.rect.width * 0.5f - (c * _rect.rect.width * 0.5f) - ((c - 1) * _rect.rect.width * HAND_SPACING * 0.5f);
      var pos = new Vector3(left + i * _rect.rect.width * (1.0f + HAND_SPACING), 0, 0);
      gameObject.TweenLocalPosition(pos, 0.25f).SetEaseCubicOut();
    }
  }
  public void Move(Transform location) {
    transform.SetParent(location, true);
    gameObject.TweenLocalPosition(Vector3.zero, 0.25f).SetEaseCubicOut();
  }
}

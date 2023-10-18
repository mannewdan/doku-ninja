using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Tweens;
using UnityEngine.UI;

public class Card : MonoBehaviour {
  const float HAND_SPACING = 0.1f; //as a percentage of card width
  const float SELECTED_OFFSET = 0.2f; //as a percentage of card height

  public CardData data {
    get { return _data; }
    set {
      _data = value;
      digit.text = _data.value.ToString();
      icon.material.SetTextureOffset("_Texture", _data.texCoords * 0.25f);
      icon.material.SetTextureOffset("_Alpha", _data.texCoords * 0.25f);
      border.color = _data.color;
    }
  }
  public Vector3 pos {
    get { return _pos + _offset; }
    set {
      _pos = value;
      gameObject.TweenCancelAll();
      gameObject.TweenLocalPosition(pos, 0.25f).SetEaseCubicOut();
    }
  }
  public Vector3 offset {
    get { return _offset; }
    set {
      _offset = value;
      gameObject.TweenCancelAll();
      gameObject.TweenLocalPosition(pos, 0.125f).SetEaseCubicOut();
    }
  }
  public DeckController deck;
  public TextMeshProUGUI digit;
  public Image icon;
  public Image border;
  public bool active {
    get { return _active; }
    set {
      if (_active != value) {
        _active = value;
        offset = _active ? Vector3.up * _rect.rect.height * SELECTED_OFFSET : Vector3.zero;
      }
    }
  }

  private CardData _data;
  private RectTransform _rect;
  private Vector3 _pos;
  private Vector3 _offset;
  private bool _active;

  void Awake() {
    _rect = transform as RectTransform;

    var mat = icon.material;
    icon.material = new Material(mat);
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
      pos = new Vector3(left + i * _rect.rect.width * (1.0f + HAND_SPACING), 0, 0);
    }
  }
  public void Move(Transform location) {
    transform.SetParent(location, true);
    pos = Vector3.zero;
  }
  public List<Point> TargetableTiles(Point origin) {
    List<Point> points = new List<Point>();
    List<Point> candidates;

    switch (data.type) {
      default:
        candidates = new List<Point>() {
          new Point(origin.x - 1, origin.y),
          new Point(origin.x, origin.y - 1),
          new Point(origin.x + 1, origin.y),
          new Point(origin.x, origin.y + 1)
        };
        break;
    }

    foreach (Point p in candidates) {
      if (deck.grid.InBounds(p)) points.Add(p);
    }

    return points;
  }
}

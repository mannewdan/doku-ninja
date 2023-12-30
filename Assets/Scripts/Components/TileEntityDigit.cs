using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileEntityDigit : MonoBehaviour {
  [SerializeField] Color textColorBombInitial;
  [SerializeField] Color textColorBombPrimed;

  public DigitDisplayMode displayMode {
    get { return _displayMode; }
    set { _displayMode = value; UpdateDigit(); }
  }
  [SerializeField] private DigitDisplayMode _displayMode = DigitDisplayMode.Hidden;

  private TileEntity owner;
  private TextMeshPro text;
  public int bombValue { get { return owner.bombValue; } }
  public int countdown { get { return owner.countdown; } }
  private Color color {
    get => _color;
    set {
      _color = value;
      var c = _color * colorMultiplier; c.a = 1;
      text.color = c;
    }
  }
  private float colorMultiplier {
    get => _colorMultiplier;
    set {
      _colorMultiplier = value;
      var c = color * _colorMultiplier; c.a = 1;
      text.color = c;
    }
  }

  private Color _color;
  private float _colorMultiplier = 1;

  void OnEnable() {
    this.AddObserver(DarkenHighlight, Notifications.SHIFT_HELD);
    this.AddObserver(ResetHighlight, Notifications.SHIFT_RELEASED);
  }
  void OnDisable() {
    this.RemoveObserver(DarkenHighlight, Notifications.SHIFT_HELD);
    this.RemoveObserver(ResetHighlight, Notifications.SHIFT_RELEASED);
  }
  protected void Awake() {
    owner = GetComponentInParent<TileEntity>();
    text = GetComponent<TextMeshPro>();
  }
  protected void Start() {
    UpdateDigit();
  }

  public void UpdateDigit() {
    if (!text) return;

    var target = countdown > 0 ? bombValue : 0;
    text.text = target > 0 ? target.ToString() : "";

    color = countdown == 2 ? textColorBombInitial : textColorBombPrimed;
  }
  private void ResetHighlight(object sender, object e) {
    colorMultiplier = 1.0f;
  }
  private void DarkenHighlight(object sender, object e) {
    colorMultiplier = 0.35f;
  }
}

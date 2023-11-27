using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Digit : MonoBehaviour {
  [SerializeField] Color textColorDefault;
  [SerializeField] Color textColorDefaultBright;
  [SerializeField] Color textColorSolution;
  [SerializeField] Color textColorFaded;
  [SerializeField] Color textColorConfirmed;
  [SerializeField] Color textColorConfirmedBright;
  [SerializeField] Color textColorWrong;
  [SerializeField] Color textColorWrongBright;
  [SerializeField] Color textColorBombInitial;
  [SerializeField] Color textColorBombPrimed;

  private Tile owner;
  private TextMeshPro text;

  public int currentDigit { get { return owner.currentDigit; } }
  public int solutionDigit { get { return owner.solutionDigit; } }
  public DigitDisplayMode displayMode {
    get { return _displayMode; }
    set { _displayMode = value; UpdateDigit(); }
  }
  [SerializeField] private DigitDisplayMode _displayMode;
  private float _initialFontSize;
  private float _initialZIndex;
  private Color _defaultColor;
  private Color _confirmedColor;
  private Color _wrongColor;

  protected void OnEnable() {
    this.AddObserver(RaiseZ, Notifications.SHIFT_HELD);
    this.AddObserver(ResetZ, Notifications.SHIFT_RELEASED);
  }
  protected void OnDisable() {
    this.RemoveObserver(RaiseZ, Notifications.SHIFT_HELD);
    this.RemoveObserver(ResetZ, Notifications.SHIFT_RELEASED);
  }

  protected void Awake() {
    owner = GetComponentInParent<Tile>();
    text = GetComponent<TextMeshPro>();
    _initialFontSize = text.fontSize;
    _initialZIndex = transform.localPosition.z;
    _defaultColor = textColorDefault;
    _confirmedColor = textColorConfirmed;
    _wrongColor = textColorWrong;
  }
  public void UpdateDigit() {
    if (!text) return;

    var target = currentDigit;
    var color = _defaultColor;
    var fontSize = _initialFontSize;
    switch (displayMode) {
      case DigitDisplayMode.Solution:
        target = solutionDigit;
        color = textColorSolution;
        break;
      case DigitDisplayMode.Faded:
        color = textColorFaded;
        break;
      case DigitDisplayMode.Confirmed:
        color = _confirmedColor;
        break;
      case DigitDisplayMode.Wall:
        color = _wrongColor;
        break;
      case DigitDisplayMode.Bomb:
        color = owner.countdown == 2 ? textColorBombInitial : textColorBombPrimed;
        fontSize = _initialFontSize * 0.66f;
        break;
    }

    text.text = target > 0 ? target.ToString() : "";
    text.color = color;
    text.fontSize = fontSize;
  }
  protected void RaiseZ(object sender, object e) {
    var pos = transform.localPosition;
    pos.z = _initialZIndex - 5.0f;
    transform.localPosition = pos;
    _defaultColor = textColorDefaultBright;
    _confirmedColor = textColorConfirmedBright;
    _wrongColor = textColorWrongBright;
    UpdateDigit();
  }
  protected void ResetZ(object sender, object e) {
    var pos = transform.localPosition;
    pos.z = _initialZIndex;
    transform.localPosition = pos;
    _defaultColor = textColorDefault;
    _confirmedColor = textColorConfirmed;
    _wrongColor = textColorWrong;
    UpdateDigit();
  }
}

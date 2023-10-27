using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Digit : MonoBehaviour {
  [SerializeField] Color textColorDefault;
  [SerializeField] Color textColorSolution;
  [SerializeField] Color textColorFaded;
  [SerializeField] Color textColorConfirmed;
  [SerializeField] Color textColorWall;
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
  private DigitDisplayMode _displayMode;
  private float _initialFontSize;

  protected void Awake() {
    owner = GetComponentInParent<Tile>();
    text = GetComponent<TextMeshPro>();
    _initialFontSize = text.fontSize;
  }
  public void UpdateDigit() {
    if (!text) return;

    var target = currentDigit;
    var color = textColorDefault;
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
        color = textColorConfirmed;
        break;
      case DigitDisplayMode.Wall:
        color = textColorWall;
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
}

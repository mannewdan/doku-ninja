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

  private Tile owner;
  private TextMeshPro text;

  public int currentDigit { get { return owner.currentDigit; } }
  public int solutionDigit { get { return owner.solutionDigit; } }
  public DigitDisplayMode displayMode {
    get { return _displayMode; }
    set { _displayMode = value; UpdateDigit(); }
  }
  private DigitDisplayMode _displayMode;

  protected void Awake() {
    owner = GetComponentInParent<Tile>();
    text = GetComponent<TextMeshPro>();
  }
  public void UpdateDigit() {
    if (!text) return;

    var target = currentDigit;
    var color = textColorDefault;
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
    }

    text.text = target > 0 ? target.ToString() : "";
    text.color = color;
  }
}

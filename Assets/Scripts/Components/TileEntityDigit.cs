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
    var color = countdown == 2 ? textColorBombInitial : textColorBombPrimed;

    text.text = target > 0 ? target.ToString() : "";
    text.color = color;
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors : MonoBehaviour {
  static Colors instance;

  [System.Serializable]
  public class ColorsUI {
    public Color white;
    public Color lightGray;
    public Color lightGrayHighlighted;
    public Color blue;
    public Color darkBlue;
  }
  [System.Serializable]
  public class ColorsText {
    public Color white;
    public Color lightGray;
    public Color blue;
  }

  [SerializeField]
  ColorsUI ui; public static ColorsUI UI { get { return instance.ui; } set { } }
  [SerializeField]
  ColorsText text; public static ColorsText Text { get { return instance.text; } set { } }

  void Awake() {
    instance = this;
  }
}
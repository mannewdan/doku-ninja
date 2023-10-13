using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors : MonoBehaviour {
  static Colors instance;

  [System.Serializable]
  public class ColorsCards {
    public Color sai;
    public Color kunai;
    public Color shuriken;
    public Color bombBox;
    public Color bombStar;
  }

  [SerializeField]
  ColorsCards cards; public static ColorsCards Cards { get { return instance.cards; } }

  void Awake() {
    instance = this;
  }
}
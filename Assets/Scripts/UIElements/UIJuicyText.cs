using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIJuicyText : MonoBehaviour {
  //essentials
  TMP_Text mesh;

  void Awake() {
    if (mesh != null) return;
    mesh = GetComponentInChildren<TMP_Text>(true);
  }

  //commands
  public void SetText(string text) {
    if (mesh == null) {
      Awake();
      if (mesh == null) return;
    }
    mesh.text = text;
  }
  public void SetColor(Color color, float duration = 0) {
    //gameObject.Tween(GetInstanceID() + "color", mesh.color, color, duration, TweenScaleFunctions.QuadraticEaseOut, (t) => { mesh.color = t.CurrentValue; });
  }
}
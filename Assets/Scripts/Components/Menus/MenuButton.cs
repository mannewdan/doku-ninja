using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MenuButtonData {
  public string name;
  public string prompt;
  public Action action;
}
public class MenuButton : MenuElement {
  public MenuButtonData data;

  public void Initialize(MenuButtonData data) {
    this.data = data;
    base.Initialize(data.name);
  }
  public override void Execute() {
    if (string.IsNullOrEmpty(data.prompt)) {
      data.action();
    } else {
      Debug.Log(data.prompt);
    }
  }
}

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

  public override void Initialize(MenuController owner, object data) {
    if (data is MenuButtonData buttonData) {
      this.data = buttonData;
      base.Initialize(owner, buttonData.name);
    }
  }
  public override void Execute() {
    if (string.IsNullOrEmpty(data.prompt)) {
      data.action();
    } else {
      owner.ConfirmationModal(data.prompt, data.action);
    }
  }
}

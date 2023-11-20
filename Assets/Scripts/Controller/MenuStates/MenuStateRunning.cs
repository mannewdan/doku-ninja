using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStateRunning : MenuState {
  protected override void OnCancel(object sender, object e) {
    owner.Close();
  }
  protected override void OnDebug(object sender, object e) {
    owner.ChangeState<MenuStateModal>();
  }
}

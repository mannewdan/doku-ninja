using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStateRunning : MenuState {
  public override void Enter() {
    base.Enter();
    owner.nextModal = null;
  }

  protected override void OnCancel(object sender, object e) {
    if (owner.inputCloseable) owner.Close();
  }
  protected override void OnDebug(object sender, object e) {
    owner.ChangeState<MenuStateModal>();
  }
}

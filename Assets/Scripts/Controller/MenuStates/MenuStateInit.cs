using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class MenuStateInit : MenuState {
  public override void Enter() {
    base.Enter();
    mainRoutine = Timing.RunCoroutine(_Start().CancelWith(gameObject));
  }

  IEnumerator<float> _Start() {
    yield return 0;
    owner.ChangeState<MenuStateRunning>();
  }
}

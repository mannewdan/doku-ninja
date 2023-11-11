using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class MenuStateRunning : MenuState {
  protected override void OnCancel(object sender, object e) {
    Timing.RunCoroutine(_Cancel().CancelWith(gameObject));
  }
  protected override void OnDebug(object sender, object e) {
    Timing.RunCoroutine(_Debug().CancelWith(gameObject));
  }

  IEnumerator<float> _Cancel() {
    yield return 0;
    owner.invoker.paused = false;
    Destroy(gameObject);
  }
  IEnumerator<float> _Debug() {
    yield return 0;
    owner.ChangeState<MenuStateModal>();
  }
}

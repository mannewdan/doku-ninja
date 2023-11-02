using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class StageStateEnvironmentStart : StageState {
  public override void Enter() {
    base.Enter();
    Timing.RunCoroutine(_Start().CancelWith(gameObject));
  }

  IEnumerator<float> _Start() {
    yield return Timing.WaitForSeconds(0.25f);

    yield return 0;
    owner.ChangeState<StageStateEnvironmentBombs>();
  }
}

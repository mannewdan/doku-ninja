using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStatePlayerWon : StageState {
  public override void Enter() {
    base.Enter();
    player.isActive = false;
    mainRoutine = Timing.RunCoroutine(_Won().CancelWith(gameObject));
  }
  public override void Exit() {
    base.Exit();
  }

  IEnumerator<float> _Won() {
    yield return 0;
  }
}

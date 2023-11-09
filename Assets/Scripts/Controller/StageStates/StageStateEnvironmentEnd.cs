using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class StageStateEnvironmentEnd : StageState {
  public override void Enter() {
    base.Enter();
    mainRoutine = Timing.RunCoroutine(_End().CancelWith(gameObject));
  }

  IEnumerator<float> _End() {
    yield return 0;
    owner.ChangeState<StageStateEnemyStart>();
  }
}

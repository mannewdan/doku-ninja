using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStateEnemyEnd : StageState {
  public override void Enter() {
    Debug.Log("End Enemy Phase");
    base.Enter();
    this.PostNotification(Notifications.ENEMY_PHASE_END);
    Timing.RunCoroutine(_End());
  }

  IEnumerator<float> _End() {
    yield return 0;
    owner.ChangeState<StageStatePlayerStart>();
  }
}

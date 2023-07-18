using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStateEnemyStart : StageState {
  public override void Enter() {
    Debug.Log("Start Enemy Phase");
    base.Enter();
    this.PostNotification(Notifications.ENEMY_PHASE_START);
    Timing.RunCoroutine(_Start());
  }

  IEnumerator<float> _Start() {
    yield return 0;
    owner.ChangeState<StageStateEnemyRounds>();
  }
}
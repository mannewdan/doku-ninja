using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStateEnemyStart : StageState {
  public override void Enter() {
    base.Enter();
    this.PostNotification(Notifications.ENEMY_PHASE_START);
    mainRoutine = Timing.RunCoroutine(_Start().CancelWith(gameObject));
  }

  IEnumerator<float> _Start() {
    yield return 0;
    owner.ChangeState<StageStateEnemyAttack>();
  }
}

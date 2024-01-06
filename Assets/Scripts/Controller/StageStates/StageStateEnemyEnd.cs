using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStateEnemyEnd : StageState {
  public override void Enter() {
    base.Enter();
    this.PostNotification(Notifications.ENEMY_PHASE_END);
    mainRoutine = Timing.RunCoroutine(_End().CancelWith(gameObject));
  }

  IEnumerator<float> _End() {
    yield return units.spawnCounter <= 0 ? Timing.WaitForSeconds(0.25f) : 0;

    units.DecrementSpawnCounter();
    yield return 0;
    owner.ChangeState<StageStatePlayerStart>();
  }
}

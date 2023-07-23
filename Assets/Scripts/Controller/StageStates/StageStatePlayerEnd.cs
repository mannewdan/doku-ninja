using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStatePlayerEnd : StageState {
  public override void Enter() {
    base.Enter();
    this.PostNotification(Notifications.PLAYER_PHASE_END);
    Timing.RunCoroutine(_End());
  }
  public override void Exit() {
    base.Exit();
    player.isActive = false;
  }

  IEnumerator<float> _End() {
    yield return 0;
    owner.ChangeState<StageStateEnemyStart>();
  }
}

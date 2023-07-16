using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStateEnemyRounds : StageState {
  public override void Enter() {
    base.Enter();
    Timing.RunCoroutine(_Loop());
  }

  IEnumerator<float> _Loop() {
    for (int i = units.enemies.Count - 1; i >= 0; i--) {
      var enemy = units.enemies[i];
      if (!enemy) continue;

      this.PostNotification(Notifications.ENEMY_ROUND_START, enemy);
      yield return 0;

      Debug.Log(enemy);
      yield return Timing.WaitForSeconds(1f);

      this.PostNotification(Notifications.ENEMY_ROUND_END, enemy);
      yield return 0;
    }

    yield return 0;
    owner.ChangeState<StageStateEnemyEnd>();
  }
}

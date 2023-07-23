using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStateEnemyAttack : StageState {
  public override void Enter() {
    base.Enter();
    Timing.RunCoroutine(_Loop());
  }

  IEnumerator<float> _Loop() {
    var enemyList = new List<UnitController>(enemies);
    enemyList.Reverse();

    for (int i = enemyList.Count - 1; i >= 0; i--) {
      var enemy = enemyList[i];
      if (!enemy || !enemy.isTelegraphing) continue;

      enemy.isActive = true;
      this.PostNotification(Notifications.ENEMY_ROUND_START, enemy);
      yield return Timing.WaitForSeconds(0.75f);
      yield return Timing.WaitUntilDone(Timing.RunCoroutine(enemy._ExecuteAttack()));
      yield return Timing.WaitForSeconds(0.75f);
      this.PostNotification(Notifications.ENEMY_ROUND_END, enemy);
      enemy.isActive = false;
      yield return 0;
    }

    yield return 0;
    owner.ChangeState<StageStateEnemyMove>();
  }
}

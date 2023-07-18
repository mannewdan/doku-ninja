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
    var enemyList = new List<UnitController>(enemies);
    enemyList.Reverse();

    for (int i = enemyList.Count - 1; i >= 0; i--) {
      var enemy = enemyList[i];
      if (!enemy) continue;

      this.PostNotification(Notifications.ENEMY_ROUND_START, enemy);
      yield return 0;

      //debug
      Debug.Log(enemy);
      yield return Timing.WaitForSeconds(0.15f);

      if (enemy.isTelegraphing) {
        enemy.ExecuteAttack();
      } else {
        //move
        enemy.QueueAttack();
      }

      this.PostNotification(Notifications.ENEMY_ROUND_END, enemy);
      yield return 0;
    }

    yield return 0;
    owner.ChangeState<StageStateEnemyEnd>();
  }
}

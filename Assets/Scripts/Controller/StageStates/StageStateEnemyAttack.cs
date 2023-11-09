using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStateEnemyAttack : StageState {
  public override void Enter() {
    base.Enter();
    mainRoutine = Timing.RunCoroutine(_Loop().CancelWith(gameObject));
  }

  IEnumerator<float> _Loop() {
    var enemyList = new List<UnitController>(enemies);
    enemyList.Reverse();

    for (int i = enemyList.Count - 1; i >= 0; i--) {
      var enemy = enemyList[i];
      if (!enemy || !enemy.isAlive) continue;

      this.PostNotification(Notifications.ENEMY_ROUND_START, enemy);
      if (enemy.targetedTiles.Contains(player.pos)) {
        enemy.isActive = true;
        yield return Timing.WaitForSeconds(0.5f);
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(enemy._ExecuteAttack().CancelWith(enemy.gameObject)));
        yield return Timing.WaitForSeconds(0.25f);
        enemy.isActive = false;
      }
      enemy.ClearAttack();
      this.PostNotification(Notifications.ENEMY_ROUND_END, enemy);
    }

    yield return enemyList.Count > 0 ? Timing.WaitForSeconds(0.25f) : 0;
    if (player.isAlive) {
      owner.ChangeState<StageStateEnemyMove>();
    } else {
      owner.ChangeState<StageStatePlayerDead>();
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStatePlayerDead : StageState {
  public override void Enter() {
    base.Enter();
    this.PostNotification(Notifications.PLAYER_DEAD);
    mainRoutine = Timing.RunCoroutine(_Dead().CancelWith(gameObject));
  }

  IEnumerator<float> _Dead() {
    yield return Timing.WaitForSeconds(0.5f);
    owner.PostNotification(Notifications.RESET);
    yield return 0;
  }
}

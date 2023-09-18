using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStatePlayerDead : StageState {
  public override void Enter() {
    base.Enter();
    this.PostNotification(Notifications.PLAYER_DEAD);
    Timing.RunCoroutine(_Dead().CancelWith(gameObject));
  }

  IEnumerator<float> _Dead() {
    Debug.Log("Game over!");
    yield return 0;
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStatePlayerEnd : StageState {
  public override void Enter() {
    Debug.Log("End Player Phase");
    base.Enter();
    this.PostNotification(Notifications.PLAYER_TURN_END);
    Timing.RunCoroutine(_End());
  }

  IEnumerator<float> _End() {
    yield return 0;
    owner.ChangeState<StageStatePlayerStart>();
  }
}

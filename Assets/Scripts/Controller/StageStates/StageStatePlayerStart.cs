using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStatePlayerStart : StageState {
  public override void Enter() {
    Debug.Log("Start Player Phase");
    base.Enter();
    this.PostNotification(Notifications.PLAYER_TURN_START);
    Timing.RunCoroutine(_Start());
  }

  IEnumerator<float> _Start() {
    yield return 0;
    owner.ChangeState<StageStatePlayerMove>();
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStatePlayerStart : StageState {
  public override void Enter() {
    base.Enter();
    this.PostNotification(Notifications.PLAYER_PHASE_START);
    Timing.RunCoroutine(_Start().CancelWith(gameObject));
    player.isActive = true;
  }

  IEnumerator<float> _Start() {
    if (deck.DrawCard()) {
      yield return Timing.WaitForSeconds(0.25f);
    }

    yield return 0;
    owner.ChangeState<StageStatePlayerMove>();
  }
}

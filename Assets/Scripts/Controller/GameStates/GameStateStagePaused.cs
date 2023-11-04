using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class GameStateStagePaused : GameState {

  public override void Enter() {
    base.Enter();
  }

  protected override void OnCancel(object sender, object e) {
    Timing.RunCoroutine(_Cancel().CancelWith(gameObject));
  }

  IEnumerator<float> _Cancel() {
    yield return 0;
    owner.ChangeState<GameStateStageRunning>();
  }
}

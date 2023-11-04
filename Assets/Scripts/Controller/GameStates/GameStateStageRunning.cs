using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class GameStateStageRunning : GameState {

  public override void Enter() {
    base.Enter();
  }

  protected override void OnStart(object sender, object e) {
    Timing.RunCoroutine(_Start().CancelWith(gameObject));
  }

  IEnumerator<float> _Start() {
    yield return 0;
    owner.ChangeState<GameStateStagePaused>();
  }
}

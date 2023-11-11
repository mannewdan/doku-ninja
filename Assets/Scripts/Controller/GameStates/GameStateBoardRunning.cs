using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class GameStateBoardRunning : GameState {
  protected override void OnStart(object sender, object e) {
    if (!owner.currentBoard || !owner.currentBoard.currentState.IsPausable()) return;

    Timing.RunCoroutine(_Start().CancelWith(gameObject));
  }

  IEnumerator<float> _Start() {
    yield return 0;
    owner.ChangeState<GameStateBoardPaused>();
  }
}

using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class GameStateBoardPaused : GameState {

  public override void Enter() {
    base.Enter();
    if (owner.currentBoard) owner.currentBoard.paused = true;
  }
  public override void Exit() {
    base.Exit();
    if (owner.currentBoard) owner.currentBoard.paused = false;
  }

  protected override void OnCancel(object sender, object e) {
    Timing.RunCoroutine(_Cancel().CancelWith(gameObject));
  }
  protected override void OnStart(object sender, object e) {
    Timing.RunCoroutine(_Cancel().CancelWith(gameObject));
  }

  IEnumerator<float> _Cancel() {
    yield return 0;
    owner.ChangeState<GameStateBoardRunning>();
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateBoardRunning : GameState {
  protected override void OnStart(object sender, object e) {
    if (!owner.currentBoard || !owner.currentBoard.currentState.IsPausable()) return;
    owner.ChangeState<GameStateBoardPaused>();
  }
}

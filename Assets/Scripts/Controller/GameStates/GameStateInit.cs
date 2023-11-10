using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class GameStateInit : GameState {
  public override void Enter() {
    base.Enter();
    mainRoutine = Timing.RunCoroutine(_Init().CancelWith(gameObject));
  }

  IEnumerator<float> _Init() {
    yield return 0;
    owner.ChangeState<GameStateBoardInit>(); //REPLACE with start menu state
  }
}

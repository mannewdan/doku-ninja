using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class StageStateEnvironmentBombs : StageState {
  public override void Enter() {
    base.Enter();
    Timing.RunCoroutine(_Countdown().CancelWith(gameObject));
  }

  IEnumerator<float> _Countdown() {
    foreach (KeyValuePair<Point, Tile> entry in grid.tiles) {
      if (entry.Value.countdown > 0) {
        entry.Value.countdown--;
        yield return Timing.WaitForSeconds(0.15f);
      }
    }

    yield return 0;
    owner.ChangeState<StageStateEnvironmentEnd>();
  }
}

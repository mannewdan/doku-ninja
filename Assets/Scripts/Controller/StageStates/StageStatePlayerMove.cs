using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStatePlayerMove : StageState {
  protected override void OnMove(object sender, object e) {
    if (e is InfoEventArgs<Point> move) {
      var newPos = player.pos + move.info;
      if (InBounds(newPos)) {
        if (apManager.SpendAP(1)) {
          player.pos = newPos;
        }
      }
    }
  }
  protected override void OnSpentAP(object sender, object e) {
    if (apManager.ap <= 0) {
      owner.ChangeState<StageStatePlayerEnd>();
    }
  }
  protected override void OnNumber(object sender, object e) {
    if (e is InfoEventArgs<int> a) {
      int val = a.info;

      owner.stateData = val;
      owner.ChangeState<StageStatePlayerCard>();
    }
  }
}

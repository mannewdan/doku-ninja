using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStatePlayerMove : StageState {
  protected override void OnMove(object sender, object e) {
    if (e is InfoEventArgs<Point> a) {
      Point delta = a.info;

      if (apManager.SpendAP(1)) {
        player.pos += delta;
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

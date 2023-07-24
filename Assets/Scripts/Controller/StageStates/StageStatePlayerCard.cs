using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStatePlayerCard : StageState {
  private int val;

  public override void Enter() {
    base.Enter();
    marker.gameObject.SetActive(true);
    pos = BestPos(player.pos, player.lastDirection);
    if (owner.stateData is int val) {
      this.val = val;
    }
  }
  public override void Exit() {
    base.Exit();
    marker.gameObject.SetActive(false);
  }

  protected override void OnMove(object sender, object e) {
    if (e is Point move) {
      pos = BestPos(player.pos, move);
    }
  }
  protected override void OnNumber(object sender, object e) {
    if (e is int number) {
      if (val == number) {
        owner.ChangeState<StageStatePlayerMove>();
      } else {
        val = number;
      }
    }
  }
  protected override void OnConfirm(object sender, object e) {
    if (apManager.SpendAP(1)) {
      var unit = units.unitMap.ContainsKey(pos) ? units.unitMap[pos] : null;
      var tile = grid.tiles.ContainsKey(pos) ? grid.tiles[pos] : null;
      if (unit) {
        unit.Harm(val);
      } else if (tile) {
        tile.currentDigit = val;
      } else {
        Debug.LogError("Couldn't find anything at position: " + pos.ToString());
      }
    }
  }
  protected override void OnSpentAP(object sender, object e) {
    if (apManager.ap <= 0) {
      owner.ChangeState<StageStatePlayerEnd>();
    } else {
      owner.ChangeState<StageStatePlayerMove>();
    }
  }
  protected override void OnCancel(object sender, object e) {
    owner.ChangeState<StageStatePlayerMove>();
  }
}

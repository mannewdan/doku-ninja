using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStatePlayerCard : StageState {
  private int val;

  public override void Enter() {
    base.Enter();
    marker.gameObject.SetActive(true);
    pos = player.pos + player.lastDirection;
    if (owner.stateData is int val) {
      this.val = val;
    }
  }
  public override void Exit() {
    base.Exit();
    marker.gameObject.SetActive(false);
  }

  protected override void OnMove(object sender, object e) {
    if (e is InfoEventArgs<Point> move) {
      pos = player.pos + move.info;
    }
  }
  protected override void OnNumber(object sender, object e) {
    if (e is InfoEventArgs<int> number) {
      if (val == number.info) {
        owner.ChangeState<StageStatePlayerMove>();
      } else {
        val = number.info;
      }
    }
  }
  protected override void OnConfirm(object sender, object e) {
    var tile = grid.tiles.ContainsKey(pos) ? grid.tiles[pos] : null;
    if (tile) {
      tile.currentDigit = val;
      owner.ChangeState<StageStatePlayerMove>();
    } else {
      Debug.Log("Couldn't find a tile at position: " + pos.ToString());
    }
  }
  protected override void OnCancel(object sender, object e) {
    owner.ChangeState<StageStatePlayerMove>();
  }
}

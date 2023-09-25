using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStatePlayerMove : StageState {
  protected override void OnMove(object sender, object e) {
    if (e is Point move) {
      var newPos = player.pos + move;

      if (IsOccupied(newPos)) return;
      if (!IsWalkable(newPos)) return;
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
    if (e is int number) {
      Card card = deck.SelectCard(number);
      if (card) {
        owner.stateData = card;
        owner.ChangeState<StageStatePlayerCard>();
      }
    }
  }
}

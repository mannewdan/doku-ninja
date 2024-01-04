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
  protected override void OnTab(object sender, object e) {
    PickCard();
  }
  protected override void OnPickCard(object sender, object e) {
    PickCard();
  }
  protected override void OnEndTurn(object sender, object e) {
    owner.ChangeState<StageStatePlayerEnd>();
  }

  void PickCard() {
    for (int i = 0; i < deck.cards.Count; i++) {
      if (deck.cards[i].active) {
        owner.stateData = deck.cards[i];
        owner.ChangeState<StageStatePlayerCard>();
      }
    }
  }
}

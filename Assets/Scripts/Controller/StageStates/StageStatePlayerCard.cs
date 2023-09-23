using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStatePlayerCard : StageState {
  private Card card;

  public override void Enter() {
    base.Enter();
    marker.gameObject.SetActive(true);
    pos = BestPos(player.pos, player.lastDirection);
    if (owner.stateData is Card card) {
      this.card = card;
    }
  }
  public override void Exit() {
    base.Exit();
    marker.gameObject.SetActive(false);
  }

  protected override void OnMove(object sender, object e) {
    if (e is Point move) {
      pos = BestPos(player.pos, move);
      player.lastDirection = (pos - player.pos).Normalized(true);
    }
  }
  protected override void OnNumber(object sender, object e) {
    if (e is int number) {
      Card newCard = deck.SelectCard(number);
      if (newCard == null) {
        return;
      } else if (card == newCard) {
        owner.ChangeState<StageStatePlayerMove>();
      } else {
        card = newCard;
      }
    }
  }
  protected override void OnConfirm(object sender, object e) {
    if (card == null) {
      Debug.Log("No card is selected!");
      owner.ChangeState<StageStatePlayerMove>();
      return;
    }

    //selection must be valid
    var tile = grid.tiles.ContainsKey(pos) ? grid.tiles[pos] : null;
    var unit = units.unitMap.ContainsKey(pos) ? units.unitMap[pos] : null;
    if (!tile) return;
    if (!unit && tile.status == TileStatus.Confirmed) return;

    if (apManager.SpendAP(1)) {
      if (unit) {
        unit.Harm(card.data.value);
      } else if (tile) {
        tile.currentDigit = card.data.value;
        if (grid.ValidateBoard(tile)) {
          Debug.Log("Player won");
        }
      } else {
        Debug.LogError("Couldn't find anything at position: " + pos.ToString());
      }

      deck.RemoveCard(card);
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

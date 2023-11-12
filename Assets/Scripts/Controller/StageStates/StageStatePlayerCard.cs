using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class StageStatePlayerCard : StageState {
  private Card card {
    get { return _card; }
    set {
      if (_card && _card.active) {
        _card.active = false;
        this.PostNotification(Notifications.CARD_INACTIVE, new TelegraphInfo(player.gameObject, _targetableTiles));
      }
      _card = value;
      if (_card && !_card.active) {
        _card.active = true;
        _targetableTiles = _card.TargetableTiles(player.pos);
        pos = BestPos(player.pos, player.lastDirection, _targetableTiles);
        this.PostNotification(Notifications.CARD_ACTIVE, new TelegraphInfo(player.gameObject, _targetableTiles));
      }
    }
  }
  private Card _card;
  private List<Point> _targetableTiles;
  public override Point pos {
    get => base.pos;
    set {
      base.pos = value;
      if (base.pos == player.pos) {
        marker.gameObject.SetActive(false);
      } else {
        marker.gameObject.SetActive(true);
      }
    }
  }

  public override void Enter() {
    base.Enter();
    marker.gameObject.SetActive(true);
    if (owner.stateData is Card card) {
      this.card = card;
    }
  }
  public override void Exit() {
    base.Exit();
    marker.gameObject.SetActive(false);
    card = null;
  }

  protected override void OnMove(object sender, object e) {
    if (e is Point move) {
      pos = BestPos(pos, move, _targetableTiles);
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
    if (pos == player.pos) return; //cards can't be placed on the player
    if (!unit && tile.status == TileStatus.Confirmed) return; //cards can't interact with confirmed digits
    if (card.data.isBomb && (unit || !tile.IsEmpty())) return; //bombs can only be placed on empty tiles
    if (apManager.HasAP(1)) {
      bool doValidation = false;
      bool validationReactsToConflicts = true;
      bool doSpendAP = true;
      if (unit) {
        unit.Harm(card.data.value);
      } else if (tile.status == TileStatus.Wall || tile.IsBomb()) {
        doValidation = true;
        bool allowConfirmation = tile.status == TileStatus.Wall;
        bool allowWalling = tile.status == TileStatus.Wall;
        tile.DamageTile(card.data.value, allowConfirmation, allowWalling);
      } else if (card.data.isBomb) {
        doValidation = true;
        validationReactsToConflicts = false;
        tile.currentDigit = card.data.value;
        tile.status = card.data.type == CardType.BoxBomb ? TileStatus.BoxBomb :
          card.data.type == CardType.StarBomb ? TileStatus.StarBomb :
          TileStatus.BoxBomb;
      } else {
        doValidation = true;
        tile.currentDigit = card.data.value;
      }

      if (doValidation && grid.ValidateBoard(tile, validationReactsToConflicts)) {
        Debug.Log("Player won");
      }

      deck.RemoveCard(card);
      if (doSpendAP) apManager.SpendAP(1);
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
  protected override void OnDiscard(object sender, object e) {
    if (card) {
      deck.RemoveCard(card);
      owner.ChangeState<StageStatePlayerMove>();
    }
  }

  public override bool IsPausable() {
    if (!base.IsPausable()) return false;
    return InputController.HardwareMode == HardwareMode.Gamepad;
  }
}

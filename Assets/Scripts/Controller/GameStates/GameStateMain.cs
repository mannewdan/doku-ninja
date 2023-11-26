using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMain : GameState {
  public override void Enter() {
    base.Enter();
    owner.currentBoard = null;
  }

  protected override void OnNumber(object sender, object e) {
    if (e is int number) {
      switch (number) {
        case 1:
          owner.boardMode = GameController.BoardMode.Stage;
          owner.ChangeState<GameStateBoardInit>();
          break;
        case 2:
          owner.boardMode = GameController.BoardMode.Edit;
          owner.ChangeState<GameStateBoardInit>();
          break;
      }
    }
  }
}

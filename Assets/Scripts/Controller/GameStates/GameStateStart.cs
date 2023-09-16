using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateStart : GameState {
  protected override void OnNumber(object sender, object e) {
    if (e is int number) {
      switch (number) {
        case 1:
          owner.ChangeState<GameStateStage>();
          break;
        case 2:
          owner.ChangeState<GameStateEdit>();
          break;
      }
    }
  }
}

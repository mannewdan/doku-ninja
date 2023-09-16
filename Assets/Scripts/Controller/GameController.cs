using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : StateMachine {
  void Start() {
    ChangeState<GameStateInit>();
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : StateMachine {
  public GameObject stageControllerPrefab;
  public GameObject editControllerPrefab;

  void Start() {
    ChangeState<GameStateInit>();
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : StateMachine {
  public GameObject stageControllerPrefab;
  public GameObject editControllerPrefab;
  public GameObject pauseMenuPrefab;
  public GameObject uiContainer;

  public StateMachineBoard currentBoard {
    get { return _currentBoard; }
    set {
      if (_currentBoard != value && _currentBoard != null) {
        Destroy(_currentBoard.gameObject);
      }
      _currentBoard = value;
    }
  }

  protected StateMachineBoard _currentBoard;

  void Start() {
    ChangeState<GameStateInit>();
  }
}

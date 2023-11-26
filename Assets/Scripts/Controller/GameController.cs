using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : StateMachine {
  public enum BoardMode { Stage, Edit }

  public GameObject stageControllerPrefab;
  public GameObject editControllerPrefab;
  public GameObject pauseMenuPrefab;
  public GameObject uiContainer;
  public BoardMode boardMode;

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

  [SerializeField] bool debugSkipToBoard;

  void Start() {
    if (debugSkipToBoard) {
      ChangeState<GameStateBoardInit>();
    } else {
      ChangeState<GameStateInit>();
    }
  }
}

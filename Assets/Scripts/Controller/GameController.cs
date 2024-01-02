using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : StateMachine {
  public enum BoardMode { Stage, Edit }

  public List<string> campaignMaps;
  public int currentMap;

  public GameObject stageControllerPrefab;
  public GameObject editControllerPrefab;
  public GameObject pauseMenuPrefab;
  public GameObject startMenuPrefab;
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

  void OnEnable() {
    this.AddObserver(OnMapSolved, Notifications.MAP_SOLVED);
  }
  void OnDisable() {
    this.RemoveObserver(OnMapSolved, Notifications.MAP_SOLVED);
  }
  void Start() {
    if (debugSkipToBoard) {
      ChangeState<GameStateBoardInit>();
    } else {
      ChangeState<GameStateInit>();
    }
  }

  void OnMapSolved(object sender, object e) {
    currentMap++;
    if (currentMap > campaignMaps.Count - 1) {
      Debug.Log("Game won!");
    } else {
      ChangeState<GameStateBoardInit>();
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class GameStateBoardPaused : GameState {
  protected GameObject pauseMenuPrefab { get { return owner.pauseMenuPrefab; } }
  protected GameObject uiContainer { get { return owner.uiContainer; } }

  protected GameObject currentMenu {
    get { return _currentMenu; }
    set {
      if (_currentMenu && _currentMenu != value) Destroy(_currentMenu);
      _currentMenu = value;
    }
  }
  private GameObject _currentMenu;

  public override void Enter() {
    base.Enter();
    if (owner.currentBoard) owner.currentBoard.paused = true;
    currentMenu = Instantiate(pauseMenuPrefab);
    currentMenu.transform.SetParent(uiContainer.transform);
    currentMenu.transform.localPosition = Vector3.zero;
  }
  public override void Exit() {
    base.Exit();
    if (owner.currentBoard) owner.currentBoard.paused = false;
    currentMenu = null;
  }

  protected override void OnCancel(object sender, object e) {
    Timing.RunCoroutine(_Cancel().CancelWith(gameObject));
  }
  protected override void OnStart(object sender, object e) {
    Timing.RunCoroutine(_Cancel().CancelWith(gameObject));
  }

  IEnumerator<float> _Cancel() {
    yield return 0;
    owner.ChangeState<GameStateBoardRunning>();
  }
}

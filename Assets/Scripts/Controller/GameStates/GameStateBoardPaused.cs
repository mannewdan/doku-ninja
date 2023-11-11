using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class GameStateBoardPaused : GameState {
  protected GameObject pauseMenuPrefab { get { return owner.pauseMenuPrefab; } }
  protected GameObject uiContainer { get { return owner.uiContainer; } }

  public override void Enter() {
    base.Enter();
    if (owner.currentBoard) owner.currentBoard.paused = true;

    GameObject newMenu = Instantiate(pauseMenuPrefab);
    newMenu.transform.SetParent(uiContainer.transform);
    newMenu.transform.localPosition = Vector3.zero;
    newMenu.transform.localScale = Vector3.one;

    MenuController menu = newMenu.GetComponent<MenuController>();
    menu.invoker = owner;
    owner.paused = true;
  }
  public override void Exit() {
    base.Exit();
    if (owner.currentBoard) owner.currentBoard.paused = false;
  }

  public override void OnUnpause() {
    owner.ChangeState<GameStateBoardRunning>();
  }
}

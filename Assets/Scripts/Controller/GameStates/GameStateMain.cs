using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMain : GameState {
  protected GameObject startMenuPrefab { get { return owner.startMenuPrefab; } }
  protected GameObject uiContainer { get { return owner.uiContainer; } }

  public override void Enter() {
    base.Enter();
    owner.currentBoard = null;

    GameObject newMenu = Instantiate(startMenuPrefab);
    newMenu.transform.SetParent(uiContainer.transform);
    newMenu.transform.localPosition = Vector3.zero;
    newMenu.transform.localScale = Vector3.one;

    MenuController menu = newMenu.GetComponent<MenuController>();
    owner.paused = true;
    menu.invoker = owner;
  }
}

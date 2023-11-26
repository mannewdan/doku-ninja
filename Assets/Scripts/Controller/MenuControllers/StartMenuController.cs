using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuController : ListMenuController {
  new protected GameController invoker { get { return (GameController)base.invoker; } }

  protected override void Start() {
    List<MenuButtonData> buttons = new List<MenuButtonData>() {
      new MenuButtonData() {
        name = "Enter Stage",
        prompt = null,
        action = () => {
          Close();
          invoker.boardMode = GameController.BoardMode.Stage;
          invoker.ChangeState<GameStateBoardInit>();
      }},
      new MenuButtonData() {
        name = "Edit Mode",
        prompt = null,
        action = () => {
          Close();
          invoker.boardMode = GameController.BoardMode.Edit;
          invoker.ChangeState<GameStateBoardInit>();
      }},
      new MenuButtonData() {
        name = "Options",
        prompt = null,
        action = () => {
          ChangeState<MenuStateModal>();
      }},
      new MenuButtonData() {
        name = "Exit Game",
        prompt = "Are you sure you want to exit the game?",
        action = () => {
          Close();
          Debug.Log("Exit Game");
          Application.Quit();
      }}
    };

    foreach (MenuButtonData data in buttons) {
      GameObject newButton = Instantiate(elementPrefab);
      newButton.transform.SetParent(elementContainer);
      newButton.transform.localPosition = Vector3.zero;
      newButton.transform.localScale = Vector3.one;

      MenuButton button = newButton.GetComponent<MenuButton>();
      button.Initialize(this, data);

      elements.Add(button);
    }

    SetInitialSelection();
    ChangeState<StartMenuStateRunning>();
  }
}

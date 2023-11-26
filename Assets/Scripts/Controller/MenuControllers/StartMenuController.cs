using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuController : ListMenuController {
  new protected GameController invoker { get { return (GameController)base.invoker; } }

  protected override void Start() {
    List<object> buttons = new List<object>() {
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

    BuildList(buttons);
    ChangeState<StartMenuStateRunning>();
  }
}

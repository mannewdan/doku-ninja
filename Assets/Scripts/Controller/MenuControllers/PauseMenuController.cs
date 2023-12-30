using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : ListMenuController {
  [SerializeField] GameObject optionsMenuPrefab;

  protected override void Start() {
    List<object> buttons = new List<object>() {
      new MenuButtonData() {
        name = "Resume",
        prompt = null,
        action = () => {
          Close();
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
    ChangeState<ListMenuStateRunning>();
  }
  protected void OldStart() {
    List<object> buttons = new List<object>() {
      new MenuButtonData() {
        name = "Resume",
        prompt = null,
        action = () => {
          Close();
      }},
      new MenuButtonData() {
        name = "Options",
        prompt = null,
        action = () => {
          nextModal = optionsMenuPrefab;
          ChangeState<MenuStateModal>();
      }},
      new MenuButtonData() {
        name = "Main Menu",
        prompt = "Are you sure you want to return to the main menu?",
        action = () => {
          Close();
          invoker.ChangeState<GameStateMain>();
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
    ChangeState<ListMenuStateRunning>();
  }
}

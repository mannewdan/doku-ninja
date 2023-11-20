using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : ListMenuController {
  protected override void Start() {
    List<MenuButtonData> buttons = new List<MenuButtonData>() {
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
          Debug.Log("Options");
      }},
      new MenuButtonData() {
        name = "Main Menu",
        prompt = "Are you sure you want to return to the main menu? Your progress on the current stage will be lost.",
        action = () => {
          Debug.Log("Main Menu");
        }},
      new MenuButtonData() {
        name = "Exit Game",
        prompt = "Are you sure you want to exit the game? Your progress on the current stage will be lost.",
        action = () => {
          Debug.Log("Exit Game");
      }}
    };

    foreach (MenuButtonData data in buttons) {
      GameObject newButton = Instantiate(elementPrefab);
      newButton.transform.SetParent(elementContainer);
      newButton.transform.localPosition = Vector3.zero;
      newButton.transform.localScale = Vector3.one;

      MenuButton button = newButton.GetComponent<MenuButton>();
      button.Initialize(data);

      elements.Add(button);
    }

    SetInitialSelection();
    ChangeState<ListMenuStateRunning>();
  }
}

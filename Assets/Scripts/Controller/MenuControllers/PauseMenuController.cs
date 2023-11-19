using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : ListMenuController {
  protected override void Start() {
    List<string> buttons = new List<string>() { "Resume", "Options", "Main Menu", "Exit Game" };

    foreach (string buttonData in buttons) {
      GameObject newButton = Instantiate(elementPrefab);
      newButton.transform.SetParent(elementContainer);
      newButton.transform.localPosition = Vector3.zero;
      newButton.transform.localScale = Vector3.one;

      MenuButton button = newButton.GetComponent<MenuButton>();
      button.Initialize(buttonData);

      elements.Add(button);
    }

    SetInitialSelection();
    ChangeState<ListMenuStateRunning>();
  }
}

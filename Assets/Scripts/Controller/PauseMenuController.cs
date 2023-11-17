using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MenuController {
  [SerializeField] Transform buttonContainer;

  protected override void Start() {
    List<string> buttons = new List<string>() { "Resume", "Options", "Main Menu", "Exit Game" };

    foreach (string buttonData in buttons) {
      GameObject newButton = Instantiate(buttonPrefab);
      newButton.transform.SetParent(buttonContainer);
      newButton.transform.localPosition = Vector3.zero;
      newButton.transform.localScale = Vector3.one;
    }

    base.Start();
  }
}

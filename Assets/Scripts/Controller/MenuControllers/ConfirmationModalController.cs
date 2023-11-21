using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmationModalController : ListMenuController {
  [SerializeField] protected TextMeshProUGUI text;

  protected string prompt;
  protected Action action;

  public void Initialize(string prompt, Action action) {
    this.prompt = prompt;
    this.action = action;

    text = GetComponentInChildren<TextMeshProUGUI>();
    if (text) text.text = prompt;
  }

  protected override void Start() {
    List<MenuButtonData> buttons = new List<MenuButtonData>() {
      new MenuButtonData() {
        name = "No",
        prompt = null,
        action = () => {
          Close();
      }},
      new MenuButtonData() {
        name = "Yes",
        prompt = null,
        action = () => {
          Close();
          action();
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
    ChangeState<ListMenuStateRunning>();
  }
}

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
    List<object> buttons = new List<object>() {
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

    BuildList(buttons);
    ChangeState<ListMenuStateRunning>();
  }
}

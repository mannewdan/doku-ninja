using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuController : ListMenuController {
  protected override void Start() {
    List<object> buttons = new List<object>() {
      new MenuButtonData() {
        name = "Resume",
        prompt = null,
        action = () => {
          Close();
      }},
    };

    BuildList(buttons);
    ChangeState<ListMenuStateRunning>();
  }
}

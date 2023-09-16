using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GameState : State {
  protected GameController owner;

  protected virtual void Awake() {
    owner = GetComponent<GameController>();
  }

  protected override void AddListeners() {
    this.AddObserver(OnNumber, Notifications.NUMBER);
  }
  protected override void RemoveListeners() {
    this.RemoveObserver(OnNumber, Notifications.NUMBER);
  }

  protected virtual void OnNumber(object sender, object e) { }
}

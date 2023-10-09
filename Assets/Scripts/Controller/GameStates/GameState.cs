using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GameState : State {
  protected GameController owner;

  protected virtual void Awake() {
    owner = GetComponent<GameController>();
  }

  protected override void AddObservers() {
    this.AddObserver(OnNumber, Notifications.NUMBER);
    this.AddObserver(OnDebug, Notifications.DEBUG);
  }
  protected override void RemoveObservers() {
    this.RemoveObserver(OnNumber, Notifications.NUMBER);
    this.RemoveObserver(OnDebug, Notifications.DEBUG);
  }

  protected virtual void OnNumber(object sender, object e) { }
  protected virtual void OnDebug(object sender, object e) { }
}

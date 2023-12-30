using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GameState : State {
  protected GameController owner;

  protected virtual void Awake() {
    owner = GetComponent<GameController>();
  }

  protected override void AddInputObservers() {
    this.AddObserver(OnStart, Notifications.START);
    this.AddObserver(OnCancel, Notifications.CANCEL);
    this.AddObserver(OnNumber, Notifications.NUMBER);
    this.AddObserver(OnDebug, Notifications.DEBUG);
    this.AddObserver(OnDebugCtrl, Notifications.DEBUG_CTRL);
  }
  protected override void RemoveInputObservers() {
    this.RemoveObserver(OnStart, Notifications.START);
    this.RemoveObserver(OnCancel, Notifications.CANCEL);
    this.RemoveObserver(OnNumber, Notifications.NUMBER);
    this.RemoveObserver(OnDebug, Notifications.DEBUG);
    this.RemoveObserver(OnDebugCtrl, Notifications.DEBUG_CTRL);
  }

  protected virtual void OnStart(object sender, object e) { }
  protected virtual void OnCancel(object sender, object e) { }
  protected virtual void OnNumber(object sender, object e) { }
  protected virtual void OnDebug(object sender, object e) { }
  protected virtual void OnDebugCtrl(object sender, object e) { }
}

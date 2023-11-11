using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : State {
  protected MenuController owner;

  protected virtual void Awake() {
    owner = GetComponent<MenuController>();
  }

  protected override void AddInputObservers() {
    this.AddObserver(OnStart, Notifications.START);
    this.AddObserver(OnCancel, Notifications.CANCEL);
    this.AddObserver(OnDebug, Notifications.DEBUG);
  }
  protected override void RemoveInputObservers() {
    this.RemoveObserver(OnStart, Notifications.START);
    this.RemoveObserver(OnCancel, Notifications.CANCEL);
    this.RemoveObserver(OnDebug, Notifications.DEBUG);
  }

  protected virtual void OnStart(object sender, object e) { }
  protected virtual void OnCancel(object sender, object e) { }
  protected virtual void OnDebug(object sender, object e) { }
}

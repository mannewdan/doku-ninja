using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : State {
  protected MenuController owner;

  protected virtual void Awake() {
    owner = GetComponent<MenuController>();
  }

  protected override void AddInputObservers() {
    this.AddObserver(OnMove, Notifications.MOVE);
    this.AddObserver(OnConfirm, Notifications.CONFIRM);
    this.AddObserver(OnStart, Notifications.START);
    this.AddObserver(OnCancel, Notifications.CANCEL);
    this.AddObserver(OnDebug, Notifications.DEBUG);
  }
  protected override void RemoveInputObservers() {
    this.RemoveObserver(OnMove, Notifications.MOVE);
    this.RemoveObserver(OnConfirm, Notifications.CONFIRM);
    this.RemoveObserver(OnStart, Notifications.START);
    this.RemoveObserver(OnCancel, Notifications.CANCEL);
    this.RemoveObserver(OnDebug, Notifications.DEBUG);
  }

  protected virtual void OnMove(object sender, object e) { }
  protected virtual void OnConfirm(object sender, object e) { }
  protected virtual void OnStart(object sender, object e) { }
  protected virtual void OnCancel(object sender, object e) { }
  protected virtual void OnDebug(object sender, object e) { }
}

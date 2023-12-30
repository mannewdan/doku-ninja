using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public enum HardwareMode { Keyboard, Gamepad };
public class InputController : MonoBehaviour {
  //fields 
  Vector2 _screenSize;
  Controls _controls;
  Repeater _up;
  Repeater _down;
  Repeater _left;
  Repeater _right;
  Repeater _tabLeft;
  Repeater _tabRight;

  //properties
  Controls controls { get { return _controls ??= new Controls(); } }
  Repeater up { get { return _up ??= new Repeater(controls, controls.general.Up.id); } }
  Repeater down { get { return _down ??= new Repeater(controls, controls.general.Down.id); } }
  Repeater left { get { return _left ??= new Repeater(controls, controls.general.Left.id); } }
  Repeater right { get { return _right ??= new Repeater(controls, controls.general.Right.id); } }
  Repeater tabLeft { get { return _tabLeft ??= new Repeater(controls, controls.general.TabLeft.id); } }
  Repeater tabRight { get { return _tabRight ??= new Repeater(controls, controls.general.TabRight.id); } }

  public static HardwareMode HardwareMode { get { return _hardwareMode; } }
  static HardwareMode _hardwareMode;

  void OnEnable() { controls.Enable(); }
  void OnDisable() { controls.Disable(); }
  void Update() {
    UpdateHardwareMode();
    UpdateScreen();
    UpdateMove();
    UpdateNumber();
    UpdateTab();
    UpdateCommand();
    UpdateShift();
  }
  void UpdateHardwareMode() {
    var keyboard = controls.FindAction(controls.general.AnyKey.id.ToString());
    var gamepad = controls.FindAction(controls.general.GamepadButton.id.ToString());

    if (gamepad != null && gamepad.WasPressedThisFrame()) {
      _hardwareMode = HardwareMode.Gamepad;
    } else if (keyboard != null && keyboard.WasPressedThisFrame()) {
      _hardwareMode = HardwareMode.Keyboard;
    }
  }
  void UpdateScreen() {
    if (_screenSize.x != Screen.width || _screenSize.y != Screen.height) {
      _screenSize = new Vector2(Screen.width, Screen.height);
      this.PostNotification(Notifications.RESIZE, _screenSize);
    }
  }
  void UpdateMove() {
    int l = left.Update(out bool leftFirstPress) ? -1 : 0;
    int r = right.Update(out bool rightFirstPress) ? 1 : 0;
    int d = down.Update(out bool downFirstPress) ? -1 : 0;
    int u = up.Update(out bool upFirstPress) ? 1 : 0;

    int x = (leftFirstPress ? l : 0) + (rightFirstPress ? r : 0);
    int y = (downFirstPress ? d : 0) + (upFirstPress ? u : 0);
    int xRepeat = l + r;
    int yRepeat = d + u;

    if (x != 0) {
      this.PostNotification(Notifications.MOVE, new Point(x, 0));
    }
    if (y != 0) {
      this.PostNotification(Notifications.MOVE, new Point(0, y));
    }
    if (xRepeat != 0) {
      this.PostNotification(Notifications.MOVE_REPEAT, new Point(xRepeat, 0));
    }
    if (yRepeat != 0) {
      this.PostNotification(Notifications.MOVE_REPEAT, new Point(0, yRepeat));
    }
  }
  void UpdateNumber() {
    var control = controls.FindAction(controls.general.Control.id.ToString());
    for (int i = 0; i <= 9; i++) {
      var action = controls.FindAction($"general/{i}");
      if (action != null && action.WasReleasedThisFrame()) {
        this.PostNotification((control != null && control.IsPressed()) ? Notifications.CONTROL_NUMBER : Notifications.NUMBER, i);
      }
    }
  }
  void UpdateTab() {
    var tab = controls.FindAction(controls.general.Tab.id.ToString());
    var shiftMod = controls.FindAction(controls.general.Shift.id.ToString());
    var tabRight = controls.FindAction(controls.general.TabRight.id.ToString());
    var tabLeft = controls.FindAction(controls.general.TabLeft.id.ToString());

    bool right = (tabRight != null && tabRight.WasReleasedThisFrame()) ||
      (tab != null && tab.WasReleasedThisFrame() && !(shiftMod != null && shiftMod.IsPressed()));
    bool left = (tabLeft != null && tabLeft.WasReleasedThisFrame()) ||
      (tab != null && tab.WasReleasedThisFrame() && shiftMod != null && shiftMod.IsPressed());

    if (left) {
      this.PostNotification(Notifications.TAB, -1);
    } else if (right) {
      this.PostNotification(Notifications.TAB, 1);
    }
  }
  void UpdateCommand() {
    var confirm = controls.FindAction(controls.general.Confirm.id.ToString());
    var cancel = controls.FindAction(controls.general.Cancel.id.ToString());
    var start = controls.FindAction(controls.general.Start.id.ToString());
    var debug = controls.FindAction(controls.general.Debug.id.ToString());
    var discard = controls.FindAction(controls.general.Discard.id.ToString());
    var save = controls.FindAction(controls.general.Save.id.ToString());
    var ctrl = controls.FindAction(controls.general.Control.id.ToString());
    var reset = controls.FindAction(controls.general.Reset.id.ToString());

    if (confirm != null && confirm.WasReleasedThisFrame()) {
      this.PostNotification(Notifications.CONFIRM, null);
    }
    if (cancel != null && cancel.WasReleasedThisFrame()) {
      this.PostNotification(Notifications.CANCEL, null);
    }
    if (start != null && start.WasReleasedThisFrame()) {
      this.PostNotification(Notifications.START, null);
    }
    if (debug != null && debug.WasReleasedThisFrame()) {
      if (ctrl != null && ctrl.IsPressed()) {
        this.PostNotification(Notifications.DEBUG_CTRL, null);
      } else {
        this.PostNotification(Notifications.DEBUG, null);
      }
    }
    if (discard != null && discard.WasReleasedThisFrame()) {
      this.PostNotification(Notifications.DISCARD, null);
    }
    if (save != null && save.WasReleasedThisFrame()) {
      this.PostNotification(Notifications.SAVE, null);
    }
    if (reset != null && reset.WasReleasedThisFrame()) {
      this.PostNotification(Notifications.RESET, null);
    }
  }
  void UpdateShift() {
    var shift = controls.FindAction(controls.general.Shift.id.ToString());
    if (shift != null) {
      if (shift.WasPressedThisFrame()) {
        this.PostNotification(Notifications.SHIFT_HELD, null);
      }
      if (shift.WasReleasedThisFrame()) {
        this.PostNotification(Notifications.SHIFT_RELEASED, null);
      }
    }
  }
}

class Repeater {
  const float Threshold = 0.35f;
  const float Rate = 0.075f;

  float _next;
  public bool _hold;
  readonly string _buttonID;
  readonly Controls _controls;

  public Repeater(Controls controls, System.Guid buttonID) {
    _controls = controls;
    _buttonID = buttonID.ToString();
  }
  public bool Update(out bool isFirst) {
    bool retValue = false;
    bool value = _controls.FindAction(_buttonID).ReadValue<float>() > 0;

    isFirst = false;
    if (value) {
      if (Time.time > _next) {
        if (!_hold) isFirst = true;
        retValue = true;
        _next = Time.time + (_hold ? Rate : Threshold);
        _hold = true;
      }
    } else {
      _hold = false;
      _next = 0;
    }

    return retValue;
  }
}
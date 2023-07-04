using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour {

  //fields 
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

  //events
  string[] _buttons = new string[] { "general/Confirm", "general/Cancel" };

  void OnEnable() { controls.Enable(); }
  void OnDisable() { controls.Disable(); }
  void Update() {
    int l = left.Update(out bool leftFirstPress) ? -1 : 0;
    int r = right.Update(out bool rightFirstPress) ? 1 : 0;
    int d = down.Update(out bool downFirstPress) ? -1 : 0;
    int u = up.Update(out bool upFirstPress) ? 1 : 0;

    int x = (leftFirstPress ? l : 0) + (rightFirstPress ? r : 0);
    int y = (downFirstPress ? d : 0) + (upFirstPress ? u : 0);
    int xRepeat = l + r;
    int yRepeat = d + u;

    if (x != 0 || y != 0) {
      this.PostNotification(Notifications.MOVE, new InfoEventArgs<Point>(new Point(x, y)));
    }
    if (xRepeat != 0 || yRepeat != 0) {
      this.PostNotification(Notifications.MOVE_REPEAT, new InfoEventArgs<Point>(new Point(xRepeat, yRepeat)));
    }
    for (int i = 0; i <= 9; i++) {
      var action = controls.FindAction($"general/{i}");
      if (action != null && action.WasReleasedThisFrame()) {
        this.PostNotification(Notifications.NUMBER, new InfoEventArgs<int>(i));
      }
    }
  }
}

class Repeater {
  const float Threshold = 0.35f;
  const float Rate = 0.075f;

  float _next;
  public bool _hold;
  string _buttonID;
  Controls _controls;

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
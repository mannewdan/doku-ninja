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
  Repeater up { get { return _up ??= new Repeater(controls, controls.ui.Up.id); } }
  Repeater down { get { return _down ??= new Repeater(controls, controls.ui.Down.id); } }
  Repeater left { get { return _left ??= new Repeater(controls, controls.ui.Left.id); } }
  Repeater right { get { return _right ??= new Repeater(controls, controls.ui.Right.id); } }
  Repeater tabLeft { get { return _tabLeft ??= new Repeater(controls, controls.ui.TabLeft.id); } }
  Repeater tabRight { get { return _tabRight ??= new Repeater(controls, controls.ui.TabRight.id); } }

  //events
  string[] _buttons = new string[] { "ui/Confirm", "ui/Cancel" };
  public static event EventHandler<InfoEventArgs<Point>> moveEvent;
  public static event EventHandler<InfoEventArgs<int>> fireEvent;

  void OnEnable() { controls.Enable(); }
  void OnDisable() { controls.Disable(); }
  void Update() {
    int x = (left.Update() ? -1 : 0) + (right.Update() ? 1 : 0);
    int y = (down.Update() ? -1 : 0) + (up.Update() ? 1 : 0);

    if (x != 0 || y != 0) {
      if (moveEvent != null) {
        moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
      }
    }

    for (int i = 0; i < _buttons.Length; i++) {
      var action = controls.FindAction(_buttons[i]);
      if (action != null && action.WasReleasedThisFrame()) {
        if (fireEvent != null) {
          fireEvent(this, new InfoEventArgs<int>(i));
        }
      }
    }
  }
}

class Repeater {
  const float Threshold = 0.5f;
  const float Rate = 0.25f;

  float _next;
  public bool _hold;
  string _buttonID;
  Controls _controls;

  public Repeater(Controls controls, System.Guid buttonID) {
    _controls = controls;
    _buttonID = buttonID.ToString();
  }
  public bool Update() {
    bool retValue = false;
    bool value = _controls.FindAction(_buttonID).ReadValue<float>() > 0;

    if (value) {
      if (Time.time > _next) {
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
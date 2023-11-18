using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : StateMachine {
  public StateMachine invoker;
  public GameObject modalPrefab;

  protected virtual void Start() {
    ChangeState<MenuStateRunning>();
  }
}

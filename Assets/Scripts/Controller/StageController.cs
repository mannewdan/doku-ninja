using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : StateMachine {
  public Grid grid;
  public GridData gridData;
  public Unit player;
  public List<Unit> enemies = new List<Unit>();

  public void Update() {

  }
}

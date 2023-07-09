using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageState : State {
  protected StageController owner;
  public Grid grid { get { return owner.grid; } }
  public UnitManager units { get { return owner.units; } }
  public Unit player { get { return units.player; } }
  public List<Unit> enemies { get { return units.enemies; } }

  protected virtual void Awake() {
    owner = GetComponent<StageController>();
  }
  protected override void AddListeners() {
    this.AddObserver(OnMove, Notifications.MOVE);
  }
  protected override void RemoveListeners() {
    this.RemoveObserver(OnMove, Notifications.MOVE);
  }
  protected virtual void OnMove(object sender, object e) {

  }
}

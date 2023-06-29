using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageState : State {
  protected StageController owner;
  public Grid grid { get { return owner.grid; } }
  public GridData gridData { get { return owner.gridData; } }
  public Unit player { get { return owner.player; } }
  public List<Unit> enemies { get { return owner.enemies; } }

  protected virtual void Awake() {
    owner = GetComponent<StageController>();
  }
  protected override void AddListeners() {
    player.AddObserver(OnMove, Notifications.MOVE);
  }
  protected override void RemoveListeners() {
    player.RemoveObserver(OnMove, Notifications.MOVE);
  }
  protected virtual void OnMove(object sender, object e) {

  }
}

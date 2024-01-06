using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : StateMachineBoard {
  public ActionPointsManager apManager;
  public DeckController deck;

  void Start() {
    LoadMap();
    ChangeState<StageStateLoad>();
  }

  void OnEnable() {
    this.AddObserver(OnMapSolved, Notifications.MAP_SOLVED);
  }
  void OnDisable() {
    this.RemoveObserver(OnMapSolved, Notifications.MAP_SOLVED);
  }

  void OnMapSolved(object sender, object e) {
    ChangeState<StageStatePlayerWon>();
  }
}

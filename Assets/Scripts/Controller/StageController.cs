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
}

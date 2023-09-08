using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : StateMachineBoard {
  public ActionPointsManager apManager;
  public DeckController deck;

  void Start() {
    Load(mapToLoad);
  }

  public void Load(string fileName) {
    MapData data = persistence.LoadMapData(fileName);
    stateData = data;
    ChangeState<StageStateLoad>();
  }
}

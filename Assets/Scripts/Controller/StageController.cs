using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : StateMachine, IPersistence {
  public Grid grid;
  public UnitManager units;
  public ActionPointsManager apManager;
  public IPersistence persistence { get { return (IPersistence)this; } }

  public string mapToLoad;

  void Start() {
    Load(mapToLoad);
  }

  public void Load(string fileName) {
    MapData data = persistence.LoadMapData(fileName);
    stateData = data;
    ChangeState<StageStateLoad>();
  }
}

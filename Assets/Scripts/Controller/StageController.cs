using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : StateMachine, IPersistence {
  public Grid grid;
  public UnitManager units;
  public IPersistence persistence { get { return (IPersistence)this; } }

  public string gridToLoad;

  void Start() {
    Load(gridToLoad);
  }

  public void Load(string fileName) {
    MapData data = persistence.LoadMapData(fileName);
    stateData = data;
    ChangeState<StageStateLoad>();
  }
}

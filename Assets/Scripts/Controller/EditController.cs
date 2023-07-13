using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditController : StateMachineBoard {
  public MapData mapData {
    get {
      if (_mapData == null) mapData = new MapData();
      grid.GatherData(ref _mapData);
      units.GatherData(ref _mapData);
      return _mapData;
    }
    set {
      _mapData = value;
      grid.Load(_mapData);
      units.Load(_mapData);
    }
  }

  [SerializeField] private MapData _mapData;

  void Start() {
    ChangeState<EditStateInit>();
  }
}

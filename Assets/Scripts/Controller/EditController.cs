using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditController : StateMachineBoard {
  public TextMeshProUGUI textEditMode;

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
    Load(mapToLoad);
    ChangeState<EditStateLoad>();
  }
  public void Save() {
    persistence.SaveMapData(mapData);
  }
}

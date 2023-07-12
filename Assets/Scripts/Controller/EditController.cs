using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditController : StateMachine, IPersistence {
  [SerializeField] GameObject selectionIndicatorPrefab;

  private Transform _marker;
  public Transform marker {
    get {
      if (_marker == null) {
        GameObject instance = Instantiate(selectionIndicatorPrefab) as GameObject;
        instance.transform.SetParent(transform);
        _marker = instance.transform;
      }
      return _marker;
    }
  }
  public Point pos;
  public Grid grid;
  public UnitManager units;
  public string mapToLoad;
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

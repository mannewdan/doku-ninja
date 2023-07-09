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
  public Grid grid;
  public Point pos;
  public string gridToLoad;
  public MapData mapData {
    get {
      grid.GatherData(ref _mapData);
      return _mapData;
    }
    set {
      _mapData = value;
      grid.Load(_mapData);
    }
  }

  [SerializeField] private MapData _mapData;

  void Start() {
    ChangeState<EditStateInit>();
  }
}

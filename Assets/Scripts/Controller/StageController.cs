using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : StateMachine, IPersistence {
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
  private Point _pos;
  public Point pos {
    get { return _pos; }
    set { _pos = value; marker.position = new Vector3(_pos.x, _pos.y); }
  }
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

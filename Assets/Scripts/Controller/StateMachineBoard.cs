using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBoard : StateMachine, IPersistence {
  [SerializeField] GameObject selectionIndicatorPrefab;

  public IPersistence persistence { get { return (IPersistence)this; } }
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
  public UnitController player { get { return units.player; } }
  public List<UnitController> enemies { get { return units.enemies; } }
  public string mapToLoad;

  public void Load(string fileName) {
    MapData data = persistence.LoadMapData(fileName);
    stateData = data;
  }
}

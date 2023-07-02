using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditModeController : MonoBehaviour {
  [SerializeField] GameObject selectionIndicatorPrefab;

  Transform _marker;
  Transform marker {
    get {
      if (_marker == null) {
        GameObject instance = Instantiate(selectionIndicatorPrefab) as GameObject;
        _marker = instance.transform;
      }
      return _marker;
    }
  }
  public Grid grid;
  public GridData gridData;
  public Point pos;
  public Dictionary<Point, TileData> tiles = new Dictionary<Point, TileData>();

  void Start() {
    grid.Initialize(6, 6);
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) {
      grid.Initialize(4, 4);
    }
  }

  public void SetGivenValue(Point p, Digit digit) {

  }
  public void SetSolutionValue(Point p, Digit digit) {

  }
  public void Save() {

  }
}

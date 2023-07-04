using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditModeController : StateMachine {
  [SerializeField] GameObject selectionIndicatorPrefab;

  Transform _marker;
  Transform marker {
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
  public GridData gridData;
  public Point pos;

  void Start() {
    if (gridData == null) gridData = new GridData();
    grid.Initialize(gridData.width, gridData.height);
    Snap();
  }
  void OnEnable() {
    this.AddObserver(OnMoveEvent, Notifications.MOVE);
    this.AddObserver(OnNumberEvent, Notifications.NUMBER);
  }
  void OnDisable() {
    this.RemoveObserver(OnMoveEvent, Notifications.MOVE);
    this.RemoveObserver(OnNumberEvent, Notifications.NUMBER);
  }
  void OnDestroy() {
    this.RemoveObserver(OnMoveEvent, Notifications.MOVE);
    this.RemoveObserver(OnNumberEvent, Notifications.NUMBER);
  }
  void OnMoveEvent(object sender, object e) {
    pos += ((InfoEventArgs<Point>)e).info;
    if (gridData != null) pos.Clamp(0, gridData.width - 1, 0, gridData.height - 1);
    Snap();
  }
  void OnNumberEvent(object sender, object e) {
    var tile = grid.tiles.ContainsKey(pos) ? grid.tiles[pos] : null;
    if (tile) {
      tile.currentDigit = ((InfoEventArgs<int>)e).info;
    } else {
      Debug.Log("Couldn't find a tile at position: " + pos.ToString());
    }
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) {
      GridData d = grid.GatherData();
      gridData = d;
    }
  }

  public void Save() {

  }
  public void Snap() {
    marker.transform.localPosition = new Vector3(pos.x, pos.y);
  }
}

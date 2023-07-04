using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditController : StateMachine {
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
  public GridData gridData;
  public Point pos;

  void Start() {
    ChangeState<EditStateInit>();
  }
  void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) {
      GridData d = grid.GatherData();
      gridData = d;
    }
  }
}

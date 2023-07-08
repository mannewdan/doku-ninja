using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DEBUG : MonoBehaviour {
  private static DEBUG instance;

  public static bool alwaysSaveToPersistentPath { get { return instance._alwaysSaveToPersistentPath; } }
  [SerializeField] bool _alwaysSaveToPersistentPath;

  void Awake() {
    SetInstance();
  }
  void OnEnable() {
    SetInstance();
  }

  void SetInstance() {
    if (instance == null || instance == this) {
      instance = this;
    } else {
      Debug.LogError("SHOULD NOT HAVE MORE THAN ONE DEBUG CLASS", this);
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageController))]
public class StageControllerInspector : Editor {
  public StageController current { get { return (StageController)target; } }
  public IPersistence persistence { get { return (IPersistence)target; } }

  public override void OnInspectorGUI() {
    DrawDefaultInspector();

    if (GUILayout.Button("Load grid")) {
      current.Load(current.gridToLoad);
    }
  }
}

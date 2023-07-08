using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EditController))]
public class EditControllerInspector : Editor {
  public EditController current { get { return (EditController)target; } }
  public IPersistence persistence { get { return (IPersistence)target; } }

  public override void OnInspectorGUI() {
    DrawDefaultInspector();

    if (GUILayout.Button("Save grid")) {
      persistence.SaveGridData(current.gridData);
    }
    if (GUILayout.Button("Load grid")) {
      GridData data = persistence.LoadGridData(current.gridToLoad);
      current.stateData = data;
      current.ChangeState<EditStateLoad>();
    }
  }
}

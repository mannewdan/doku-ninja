using UnityEngine;
using UnityEditor;

public class DeselectHotkey : ScriptableObject {
    [MenuItem("Custom/Deselect All _&d")]
    static void DoDeselect() {
        Selection.objects = new UnityEngine.Object[0];
    }
}
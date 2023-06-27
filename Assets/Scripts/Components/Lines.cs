using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lines : MonoBehaviour {
  [SerializeField] GameObject linePrefab;
  [SerializeField] float tipLength;
  [SerializeField] float overshoot;

  List<LineRenderer> lines = new List<LineRenderer>();
  float orthoSize = 0;

  void Update() {
    if (Camera.main.orthographicSize != orthoSize) {
      orthoSize = Camera.main.orthographicSize;
      float newWidth = orthoSize / 100.0f;

      foreach (LineRenderer line in lines) {
        line.widthMultiplier = newWidth;
      }
    }
  }

  public void Initialize(int width, int height) {
    if (!linePrefab) return;

    //vertical lines
    for (int x = 0; x < width + 1; x++) {
      Vector3[] points = new Vector3[4];
      points[0] = new Vector2(x - 0.5f, -(0.5f + overshoot));
      points[1] = new Vector2(x - 0.5f, -(0.5f + overshoot - tipLength));
      points[2] = new Vector2(x - 0.5f, height - 1f + (0.5f + overshoot - tipLength));
      points[3] = new Vector2(x - 0.5f, height - 1f + (0.5f + overshoot));
      BuildLine(points);
    }

    //horizontal lines
    for (int y = 0; y < height + 1; y++) {
      Vector3[] points = new Vector3[4];
      points[0] = new Vector2(-(0.5f + overshoot), y - 0.5f);
      points[1] = new Vector2(-(0.5f + overshoot - tipLength), y - 0.5f);
      points[2] = new Vector2(width - 1f + (0.5f + overshoot - tipLength), y - 0.5f);
      points[3] = new Vector2(width - 1f + (0.5f + overshoot), y - 0.5f);
      BuildLine(points);
    }
  }
  void BuildLine(Vector3[] points) {
    GameObject newLine = Instantiate(linePrefab);
    newLine.transform.parent = transform;
    newLine.transform.localPosition = Vector3.zero;
    newLine.transform.localEulerAngles = Vector3.zero;

    LineRenderer lRenderer = newLine.GetComponent<LineRenderer>();
    if (lRenderer) {
      lRenderer.SetPositions(points);
      lines.Add(lRenderer);
    }
  }
}

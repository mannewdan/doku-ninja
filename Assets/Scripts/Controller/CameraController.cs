using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
  Camera _camera;

  void Awake() {
    _camera = GetComponent<Camera>();
  }
  void OnEnable() {
    this.AddObserver(UpdatePosition, Notifications.MAP_SIZE_CHANGED);
  }
  void OnDisable() {
    this.RemoveObserver(UpdatePosition, Notifications.MAP_SIZE_CHANGED);
  }

  void UpdatePosition(object sender, object e) {
    if (e is Point size) {
      var pos = transform.localPosition;
      pos.x = size.x * 0.5f - 0.5f;

      if (size.y >= 9) {
        pos.y = 2;
        _camera.orthographicSize = 7.75f;
      } else if (size.y >= 6) {
        pos.y = 1;
        _camera.orthographicSize = 5.5f;
      } else {
        pos.y = 0.35f;
        _camera.orthographicSize = 4.0f;
      }

      transform.localPosition = pos;
    }
  }
}

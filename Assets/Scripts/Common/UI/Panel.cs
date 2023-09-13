using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LayoutAnchor))]
public class Panel : MonoBehaviour {
  [System.Serializable]
  public class Position {
    public string name;
    public TextAnchor anchor;
    public TextAnchor parentAnchor;
    public Vector2 offset;

    public Position(string name) { this.name = name; }
    public Position(string name, TextAnchor anchor, TextAnchor parentAnchor) : this(name) {
      this.anchor = anchor;
      this.parentAnchor = parentAnchor;
    }
    public Position(string name, TextAnchor anchor, TextAnchor parentAnchor, Vector2 offset) : this(name, anchor, parentAnchor) {
      this.offset = offset;
    }
  }

  [SerializeField] List<Position> positionList;
  Dictionary<string, Position> positionMap;
  LayoutAnchor anchor;

  public Position CurrentPosition { get; private set; }

  void Awake() {
    anchor = GetComponent<LayoutAnchor>();
    positionMap = new Dictionary<string, Position>(positionList.Count);
    for (int i = positionList.Count - 1; i >= 0; i--) {
      AddPosition(positionList[i]);
    }
  }
  void Start() {
    if (CurrentPosition == null && positionList.Count > 0)
      SetPosition(positionList[0], false);

    this.AddObserver(OnResize, Notifications.RESIZE);
  }
  void OnDisable() {
    this.RemoveObserver(OnResize, Notifications.RESIZE);
  }
  void OnDestroy() {
    this.RemoveObserver(OnResize, Notifications.RESIZE);
  }

  void OnResize(object sender, object e) {
    anchor.SnapToAnchorPosition(CurrentPosition.anchor, CurrentPosition.parentAnchor, CurrentPosition.offset);
  }

  public Position this[string name] { get { return positionMap.ContainsKey(name) ? positionMap[name] : null; } }
  public void AddPosition(Position p) { positionMap[p.name] = p; }
  public void RemovePosition(Position p) {
    if (positionMap.ContainsKey(p.name)) {
      positionMap.Remove(p.name);
    }
  }

  public Tweens.Core.Tween<Vector2> SetPosition(string positionName, bool animated = false) {
    return SetPosition(this[positionName], animated);
  }
  public Tweens.Core.Tween<Vector2> SetPosition(Position p, bool animated = false) {
    CurrentPosition = p;
    if (CurrentPosition == null) return null;

    if (animated) {
      return anchor.MoveToAnchorPosition(p.anchor, p.parentAnchor, p.offset);
    } else {
      anchor.SnapToAnchorPosition(p.anchor, p.parentAnchor, p.offset);
    }

    return null;
  }

}

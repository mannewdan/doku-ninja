using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

[RequireComponent(typeof(RectTransform))]
public class LayoutAnchor : MonoBehaviour {
  RectTransform self;
  RectTransform parent;

  void Awake() {
    self = transform as RectTransform;
    parent = transform.parent as RectTransform;
    if (parent == null) {
      Debug.LogError("This component requires a RectTransform parent to work", gameObject);
    }
  }

  Vector2 GetPosition(RectTransform rt, TextAnchor anchor) {
    Vector2 retValue = Vector2.zero;
    switch (anchor) {
      case TextAnchor.LowerCenter:
      case TextAnchor.MiddleCenter:
      case TextAnchor.UpperCenter:
        retValue.x += rt.rect.width * 0.5f;
        break;
      case TextAnchor.LowerRight:
      case TextAnchor.MiddleRight:
      case TextAnchor.UpperRight:
        retValue.x += rt.rect.width;
        break;
    }
    switch (anchor) {
      case TextAnchor.MiddleLeft:
      case TextAnchor.MiddleCenter:
      case TextAnchor.MiddleRight:
        retValue.y += rt.rect.height * 0.5f;
        break;
      case TextAnchor.UpperLeft:
      case TextAnchor.UpperCenter:
      case TextAnchor.UpperRight:
        retValue.y += rt.rect.height;
        break;
    }
    return retValue;
  }
  public Vector2 AnchorPosition(TextAnchor anchor, TextAnchor anchorParent, Vector2 offset) {
    Vector2 myOffset = GetPosition(self, anchor);
    Vector2 parentOffset = GetPosition(parent, anchorParent);
    Vector2 anchorCenter = new Vector2(Mathf.Lerp(self.anchorMin.x, self.anchorMax.x, self.pivot.x), Mathf.Lerp(self.anchorMin.y, self.anchorMax.y, self.pivot.y));
    Vector2 myAnchorOffset = new Vector2(parent.rect.width * anchorCenter.x, parent.rect.height * anchorCenter.y);
    Vector2 myPivotOffset = new Vector2(self.rect.width * self.pivot.x, self.rect.height * self.pivot.y);
    Vector2 pos = parentOffset - myAnchorOffset - myOffset + myPivotOffset + offset;
    pos.x = Mathf.RoundToInt(pos.x);
    pos.y = Mathf.RoundToInt(pos.y);
    return pos;
  }
  public void SnapToAnchorPosition(TextAnchor anchor, TextAnchor anchorParent, Vector2 offset) {
    self.anchoredPosition = AnchorPosition(anchor, anchorParent, offset);
  }
  public Tweens.Core.Tween<Vector2> MoveToAnchorPosition(TextAnchor anchor, TextAnchor anchorParent, Vector2 offset) {
    return self.gameObject.TweenAnchoredPosition(AnchorPosition(anchor, anchorParent, offset), 0.35f).SetEaseCubicOut();
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListMenuController : MenuController {
  [SerializeField] protected GameObject elementPrefab;
  [SerializeField] protected Transform elementContainer;

  protected List<MenuElement> elements = new List<MenuElement>();

  public int selection {
    get { return _selection; }
    set {
      if (_selection >= 0 && elements.Count > 0 && _selection < elements.Count) elements[_selection].selected = false;

      _selection = value;
      if (_selection < 0) _selection = elements.Count - 1;
      if (_selection > elements.Count - 1) _selection = 0;

      if (elements.Count > 0) elements[_selection].selected = true;
    }
  }
  protected int _selection;
}

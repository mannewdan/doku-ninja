using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListMenuController : MenuController {
  protected const int NULL_SELECTION = -10;

  [SerializeField] protected GameObject elementPrefab;
  [SerializeField] protected Transform elementContainer;

  protected List<MenuElement> elements = new List<MenuElement>();

  public int selection {
    get { return _selection; }
    set {
      if (_selection >= 0 && elements.Count > 0 && _selection < elements.Count) elements[_selection].selected = false;

      if (_selection == NULL_SELECTION) {
        _selection = 0;
      } else {
        _selection = value;
      }

      if (_selection == NULL_SELECTION) return;
      if (_selection < 0) _selection = elements.Count - 1;
      if (_selection > elements.Count - 1) _selection = 0;
      if (elements.Count > 0) elements[_selection].selected = true;
    }
  }
  protected int _selection;
  public MenuElement currentElement {
    get { return (selection >= 0 && selection < elements.Count) ? elements[selection] : null; }
  }

  protected void BuildList(List<object> datas) {
    foreach (object data in datas) {
      GameObject newElement = Instantiate(elementPrefab);
      newElement.transform.SetParent(elementContainer);
      newElement.transform.localPosition = Vector3.zero;
      newElement.transform.localScale = Vector3.one;

      MenuElement element = newElement.GetComponent<MenuElement>();
      element.Initialize(this, data);
      elements.Add(element);
    }

    selection = InputController.HardwareMode == HardwareMode.Gamepad ? 0 : NULL_SELECTION;
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListMenuController : MenuController {
  [SerializeField] protected GameObject elementPrefab;
  [SerializeField] protected Transform elementContainer;

  protected List<MenuElement> elements = new List<MenuElement>();

  //inputs for changing selection

  //inputs for confirmation

  //inputs for secondary selection (horizontal inputs on options for example)
}

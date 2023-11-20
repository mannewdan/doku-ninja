using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuElement : MonoBehaviour {
  [SerializeField] protected Image background;
  [SerializeField] protected Image border;
  [SerializeField] protected TextMeshProUGUI text;

  [SerializeField] protected Color selectedBorderColor;
  [SerializeField] protected Color defaultBorderColor;

  public bool selected {
    get { return _selected; }
    set {
      _selected = value;
      if (border) border.color = _selected ? selectedBorderColor : defaultBorderColor;
    }
  }
  public bool clickable { get { return _clickable; } set { _clickable = value; } }

  protected bool _selected;
  protected bool _clickable;

  void Awake() {
    background = GetComponent<Image>();
    border = GetComponentsInChildren<Image>().First((img) => { return img.name == "Border"; });
    text = GetComponentInChildren<TextMeshProUGUI>();
  }

  public virtual void Initialize(string name) {
    if (text) text.text = name;
  }
  public abstract void Execute();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {
  public CardData data;
  public DeckController deck;

  public void Move(Transform location, bool spread = false) {
    transform.SetParent(location, true);
    transform.localPosition = Vector3.zero;
  }
}

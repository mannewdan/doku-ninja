using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData {
  public int value;
  public Color color;
  public CardType type;
  public Vector2 texCoords;

  public CardData(int value, CardType type) {
    this.value = value;
    this.type = type;

    switch (type) {
      case CardType.Sai:
        texCoords = new Vector2(0, 0);
        color = Colors.Cards.sai;
        break;
      case CardType.Kunai:
        texCoords = new Vector2(1, 0);
        color = Colors.Cards.kunai;
        break;
      case CardType.Shuriken:
        texCoords = new Vector2(2, 0);
        color = Colors.Cards.shuriken;
        break;
      case CardType.BoxBomb:
        texCoords = new Vector2(0, 1);
        color = Colors.Cards.bombBox;
        break;
      case CardType.StarBomb:
        texCoords = new Vector2(1, 1);
        color = Colors.Cards.bombStar;
        break;
    }
  }
}
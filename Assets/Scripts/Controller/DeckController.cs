using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour {
  public Grid grid;
  public UnitManager units;
  [SerializeField] GameObject cardPrefab;

  public List<Card> cards = new List<Card>();

  public void BuildDeck(int size) {
    for (int i = cards.Count - 1; i >= 0; i--) {
      if (cards[i] != null) {
        Destroy(cards[i].gameObject);
      }
    }
    cards.Clear();
    for (int i = 0; i < size; i++) {
      Card card = NewCard(new CardData(i + 1, CardType.Sai));
      card.Initialize(i, size);
      cards.Add(card);
    }
  }
  Card NewCard(CardData data) {
    GameObject newCard = Instantiate(cardPrefab);
    newCard.transform.SetParent(transform);
    newCard.transform.localPosition = Vector3.zero;
    newCard.transform.localEulerAngles = Vector3.zero;
    newCard.transform.localScale = Vector3.one;

    Card card = newCard.GetComponentInChildren<Card>();
    card.deck = this;
    card.data = data;

    return card;
  }

  public Card SelectCard(int input) {
    int index = IndexOfInput(input);
    if (index >= 0 && index < cards.Count) {
      return cards[index].active ? cards[index] : null;
    } else return null;
  }
  public bool DrawCards() {
    bool cardDrawn = false;
    foreach (Card card in cards) {
      if (card.cooldown > 0) {
        card.cooldown--;
        cardDrawn = true;
      }
    }
    return cardDrawn;
  }
  public void RemoveCard(Card card) {
    card.active = false;
  }

  int IndexOfInput(int input) {
    if (input <= 0) input = 10;
    return input - 1;
  }
}

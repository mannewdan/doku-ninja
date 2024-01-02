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

    Card card = newCard.GetComponent<Card>();
    card.transform.localPosition = Vector3.zero;
    card.transform.localEulerAngles = Vector3.zero;
    card.transform.localScale = Vector3.one;

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
  public bool DrawCard() {
    List<Card> candidates = new List<Card>();
    foreach (Card c in cards) {
      if (!c.active) candidates.Add(c);
    }

    if (candidates.Count > 0) {
      candidates[Random.Range(0, candidates.Count)].active = true;

      return true;
    } else return false;
  }
  public void RemoveCard(Card card) {
    card.active = false;
  }

  int IndexOfInput(int input) {
    if (input <= 0) input = 10;
    return input - 1;
  }
}

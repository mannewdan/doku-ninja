using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour {
  const int MAX_CARDS_IN_HAND = 3;

  [SerializeField] GameObject cardPrefab;

  public List<Card> hand = new List<Card>();
  public List<Card> deck = new List<Card>();
  public List<Card> dead = new List<Card>();

  void Start() {
    //debug
    for (int i = 0; i < 15; i++) {
      deck.Add(NewCard());
    }
    for (int i = 0; i < MAX_CARDS_IN_HAND; i++) {
      DrawCard();
    }
  }

  public bool DrawCard() {
    if (deck.Count == 0) return false;
    var card = deck[0];
    hand.Add(card);
    deck.RemoveAt(0);
    return true;
  }
  public void ShuffleDeck(bool fromDead) {
    if (fromDead) {
      deck.AddRange(dead);
      dead.Clear();
    }
    deck = U.Shuffle(deck);
  }

  Card NewCard() {
    GameObject newCard = Instantiate(cardPrefab);
    newCard.transform.SetParent(transform);

    Card card = newCard.GetComponent<Card>();

    card.transform.localPosition = Vector3.zero;
    card.transform.localEulerAngles = Vector3.zero;
    card.transform.localScale = Vector3.one;

    return card;
  }
}

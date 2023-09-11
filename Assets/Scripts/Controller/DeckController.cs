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
    //initialize deck
    for (int i = 0; i < 15; i++) {
      deck.Add(NewCard());
    }

    ShuffleDeck();

    for (int i = 0; i < MAX_CARDS_IN_HAND; i++) {
      DrawCard();
    }
  }

  public bool DrawCard() {
    if (deck.Count == 0) {
      Debug.Log("Did not draw a card -- deck is empty.");
      return false;
    }
    if (hand.Count >= MAX_CARDS_IN_HAND) {
      Debug.Log("Did not draw a card -- hand is full.");
      return false;
    }

    var card = deck[0];
    hand.Add(card);
    deck.RemoveAt(0);
    return true;
  }
  public void ShuffleDeck() {
    deck = U.Shuffle(deck);
  }
  public void ShuffleGraveyard() {
    deck.AddRange(dead);
    dead.Clear();
    ShuffleDeck();
  }
  public Card SelectCard(int input) {
    if (input == 0) input = 10;
    int index = input - 1;
    if (index >= 0 && index < hand.Count) {
      return hand[index];
    } else return null;
  }
  public bool RemoveCard(Card card) {
    if (!hand.Contains(card)) {
      Debug.Log("Tried to remove a card that is not in the hand!");
      return false;
    }
    hand.Remove(card);
    dead.Add(card);
    return true;
  }

  Card NewCard() {
    GameObject newCard = Instantiate(cardPrefab);
    newCard.transform.SetParent(transform);

    Card card = newCard.GetComponent<Card>();

    card.transform.localPosition = Vector3.zero;
    card.transform.localEulerAngles = Vector3.zero;
    card.transform.localScale = Vector3.one;

    card.data = new CardData() { value = Random.Range(1, 7), type = CardType.Default };

    return card;
  }
}

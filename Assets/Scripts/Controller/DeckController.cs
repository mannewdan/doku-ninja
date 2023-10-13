using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour {
  const int MAX_CARDS_IN_HAND = 3;

  [SerializeField] GameObject cardPrefab;
  [SerializeField] Transform handT;
  [SerializeField] Transform deadT;

  public List<Card> hand = new List<Card>();
  public List<Card> deck = new List<Card>();
  public List<Card> dead = new List<Card>();

  void Start() {
    //initialize deck
    for (int t = 0; t < System.Enum.GetValues(typeof(CardType)).Length; t++) {
      for (int i = 0; i < 4; i++) {
        deck.Add(NewCard(new CardData(i + 1, (CardType)t)));
      }
    }

    ShuffleDeck();

    for (int i = 0; i < MAX_CARDS_IN_HAND; i++) {
      DrawCard();
    }
  }

  public bool DrawCard() {
    if (deck.Count == 0) return false;
    if (hand.Count >= MAX_CARDS_IN_HAND) return false;

    var card = deck[0];
    hand.Add(card);
    card.Move(handT);
    deck.RemoveAt(0);
    this.PostNotification(Notifications.CARD_DRAW);
    return true;
  }
  public void ShuffleDeck() {
    deck = U.Shuffle(deck);
  }
  public void ShuffleGraveyard() {
    deck.AddRange(dead);
    dead.ForEach(c => c.Move(transform));
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
    card.Move(deadT);
    this.PostNotification(Notifications.CARD_DISCARD);
    return true;
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
    card.Move(transform);

    return card;
  }
}

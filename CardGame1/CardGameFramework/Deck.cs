using System;
using System.Collections.Generic;

namespace BasicCard.CardGameFramework
{
    public class Deck
    {
        // Create a list of cards
        protected List<Card> cards = new List<Card>();

        // Returns the card at the given position
        public Card this[int position] { get { return (Card)cards[position]; } }

        // The get method returns the value of the variable name. The set method assigns a value to the name variable.

        public Deck()
        {
            // Complete deck with every face value and suit

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (FaceValue faceVal in Enum.GetValues(typeof(FaceValue)))
                {
                    cards.Add(new Card(suit, faceVal, true));
                }
            }
        }

        public Card Draw()
        {
            // Draws one card and removes it from the deck
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public void ShuffleCards()
        {
            // Shuffle the cards in the deck, using the Fisher-Yates shuffle algorithm
            int i;
            int j;
            int MAXCARDS = 52;

            Random sortRandom = new Random();

            for (i = 0; i <= (MAXCARDS - 1); i++)
            {
                j = Convert.ToInt32(sortRandom.Next(0, i + 1));
                var deck = cards[i];
                cards[i] = cards[j];
                cards[j] = deck;
            }
        }

        public void Shuffle()
        {
            // Shuffle the cards in the deck
            Random random = new Random();
            for (int i = 0; i < cards.Count; i++)
            {
                int index1 = i;
                int index2 = random.Next(cards.Count);
                SwapCard(index1, index2);
            }
        }

        private void SwapCard(int index1, int index2)
        {
            // Swap placement of two cards
            Card card = cards[index1];
            cards[index1] = cards[index2];
            cards[index2] = card;
        }
    }
}

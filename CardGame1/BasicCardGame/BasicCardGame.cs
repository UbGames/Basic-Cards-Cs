using System.Collections.Generic;
using BasicCard.CardGameFramework;

namespace BasicCard
{
    public class BasicCardGame
    {
        // Private Deck and Player objects for the current deck and dealer
        private Deck deck;
        private readonly Player dealer;

        // The get method returns the value of the variable name. The set method assigns a value to the name variable.

        // Public properties to return the dealer, and current deck
        public Player Dealer { get { return dealer; } }
        public Deck CurrentDeck { get { return deck; } }

        public BasicCardGame()
        {
            // Create a dealer
            dealer = new Player();
        }

        public void ShuffleNewCards()
        {
            // Create a new deck and then shuffle the deck
            deck = new Deck();
            deck.Shuffle();

            // Fisher - Yates shuffle algorithm
            //deck.ShuffleCards();
        }

        public void ShowNewDeck()
        {
            // Create a new unshuffle deck of cards
            deck = new Deck();
        }

        public void DealNewGame(int NumberOfCards)
        {
            // Deals a new game. This is invoked through the New Deck button and Shuffle and Deal button in BasicCardForm.cs

            // Reset the dealer hand in case this is not the first game
            dealer.NewHand();

            // Deal 52 cards, 0-51
            for (int i = 0; i <= NumberOfCards; i++)
            {
                Card d = deck.Draw();
                // Set the dealer cards to be facing up
                d.IsCardUp = true;
                dealer.Hand.Cards.Add(d);
            }

            // Give the dealer a handle to the current deck
            dealer.CurrentDeck = deck;
        }

        // Used for testing sending and receiving parameters in function
        public int AddTwoNumbers(int number1, int number2)
        {
            return number1 + number2;
        }

    }
}

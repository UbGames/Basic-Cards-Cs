using System.Collections.Generic;
using BasicCard.CardGameFramework;
using System.Windows.Forms;
using System;

namespace BasicCard
{
    public class Player
    {
        // Player information
        private BasicCardHand hand;
        private Deck currentDeck;

        // The get method returns the value of the variable name. The set method assigns a value to the name variable.

        public Deck CurrentDeck { get { return currentDeck; } set { currentDeck = value; } }
        public BasicCardHand Hand { get { return hand; } }

        public BasicCardHand NewHand()
        {
            // Creates a new hand for the current player, returns BasicCardHand
            this.hand = new BasicCardHand();
            return this.hand;
        }
    }
}

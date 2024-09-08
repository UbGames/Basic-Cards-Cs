using System;
using System.Collections.Generic;

namespace BasicCard.CardGameFramework
{
    public class Hand
    {
        // Create a list of cards
        protected List<Card> cards = new List<Card>();
        public int NumCards { get { return cards.Count; } }
        public List<Card> Cards { get { return cards; } }

        // The get method returns the value of the variable name. The set method assigns a value to the name variable.
    }

    public class BasicCardHand : Hand
    {
        // This method could be used to compare two hands of cards
        //public int CompareFaceValue(object otherHand)
        //{
        //}
    }
}

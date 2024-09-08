
namespace BasicCard.CardGameFramework
{
    public enum Suit
    {
        // Card suit values
        Diamonds, Spades, Clubs, Hearts
    }

    public enum FaceValue
    {
        // Card face values
        Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8,
        Nine = 9, Ten = 10, Jack = 11, Queen = 12, King = 13, Ace = 14
    }

    public class Card
    {
        // Card information
        private readonly Suit suit;
        private readonly FaceValue faceVal;
        private bool isCardUp;

        // The public property Suit is associated with the suit field, FaceValue is associated with the faceVal field.
        // The get method returns the value of the variable name. The set method assigns a value to the name variable.

        public Suit Suit { get { return suit; } }
        public FaceValue FaceVal { get { return faceVal; } }
        public bool IsCardUp { get { return isCardUp; } set { isCardUp = value; } }

        public Card(Suit suit, FaceValue faceVal, bool isCardUp)
        {
            // Assign the card a suit, face value, and if the card is facing up or down
            this.suit = suit;
            this.faceVal = faceVal;
            this.isCardUp = isCardUp;
        }

        public override string ToString()
        {
            // Return the card as a string (i.e. "The Ace of Spades")
            return "The" + faceVal.ToString() + "of" + suit.ToString();
        }
    }
}
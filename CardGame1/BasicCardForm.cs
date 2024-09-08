using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BasicCard.CardGameFramework;
using System.Media;

namespace BasicCard
{
    partial class BasicCardForm : Form
    {
        //BasicCardsOOP
        // Create a new card game with one player
        private readonly BasicCardGame game = new BasicCardGame();

        private PictureBox[] dealerCards;

        private bool firstTurn;

        // Set value of animation interval, the lower the number the faster the dealing cards
        private readonly int AnimationTimerInterval = 15;
        private bool AnimationDone = false;

        private int DealerCardLocationX, DealerCardLocationY;

        public int MAXCARDS = 52;
        public int CardsInDeck;
        public int CardsInDealersHand;

        // Initial numbers of cards in initial hand, 52 cards, 0-4, 12, 25, 38, 51
        private readonly int NumberOfCards = 51;
        // How fast to deal cards, 50
        private readonly int DealSpeed = 70;

        private bool FlagShuffleCards;

        // Start new game
        private bool NewGameStarted;

        // Fields needed for card dealing animation
        private Point _deckstartPoint;
        private int _DealerCardIndex;
        private int _a, _b;
        private int _x, _y;
        private int _Increment;
        private int _x1, _y1;

        // The @ which will allow us the use of single slashes instead of using double slashes. Path is set to the sound folder under the bin folder.
        public const string SoundFileFolder = @"Sounds\";

        // Location to the sound file
        public static object SoundFileLocation;

        // Turn sound on/off, 1,0, default off
        private readonly short NSound = 1;

        // Used for testing card game, displays the number of cards left in deck, number of cards dealt
        // Set to true to show labels, set to false to hide labels, set to false to hide labels when going live.
        private readonly bool tFlag = true;

        public BasicCardForm()
        {
            // Initializes components, loads the card skin images, and setup new game
            InitializeComponent();
            LoadCardSkinImages();
            SetUpNewGame();
        }
       
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Override OnFormClosing by doing the following. Just be careful you do not do anything unexpected, as clicking the 'X' to close is a well understood behavior.

            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            // Confirm user wants to close
            switch (MessageBox.Show(this, "Are you sure you want to exit?", "Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    break;
            }
        }

        private void BasicCardForm_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;

            AnimationTimerDealer.Interval = AnimationTimerInterval;
            AnimationTimerDealer.Tick += new EventHandler(AnimateDealerCardTimer_Tick);

            // DeckCard
            deckCard1.Hide();
            DeckCardPB.Location = deckCard1.Location;
            DeckCardPB.BringToFront();
            DeckCardPB.Hide();

            // Save the original location of the DeckCardPB pictureBox
            _deckstartPoint = DeckCardPB.Location;
            _x = DeckCardPB.Location.X;
            _y = DeckCardPB.Location.Y;
            _Increment = 0;

            CardsInDeck = MAXCARDS;
            // Start new game
            NewGameStarted = false;
            dealButton.Enabled = false;
            FlagShuffleCards = true;

            lblNew.ForeColor = Color.Gold;
            lblDeck.ForeColor = Color.White;
            lblDealer.ForeColor = Color.White;
            lblCardsInDeck.ForeColor = Color.White;
            lblCardsInDealersHand.ForeColor = Color.White;

            lblNew.Text = "";
            lblCardsInDeck.Text = "0";
            lblCardsInDealersHand.Text = "0";

            lblNew.Hide();
            lblDeck.Hide();
            lblDealer.Hide();
            lblCardsInDeck.Hide();
            lblCardsInDealersHand.Hide();

            dealButton.BackColor = Color.Gold;
            ShowHandsBtn.BackColor = Color.Gold;
            HideHandsBtn.BackColor = Color.Gold;
            clearTableButton.BackColor = Color.Gold;
            NewDeckBtn.BackColor = Color.Gold;
            endGameButton.BackColor = Color.Gold;

        }

        private void DealBtn_Click(object sender, EventArgs e)
        {
            // Invoked when the deal button is clicked
            try
            {
                // Ask to shuffle and deal the cards - dialog box with two buttons: yes and no.
                DialogResult result1 = MessageBox.Show("Would you like to shuffle and deal the cards?", "Shuffle and Deal Cards", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //MessageBox.Show("result1 " + result1, "Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                if ((result1 == DialogResult.Yes))
                {
                    // Call to Shuffle Cards, DealNewGame is in BasicCardGames.cs
                    if ((CardsInDeck <= 52 || CardsInDeck == MAXCARDS) && (FlagShuffleCards == true) && (NewGameStarted == true))
                    {
                        PlaySoundFiles("shuffling_cards");

                        // Card delay of 1.0 sec
                        Delay(500);

                        // Refers to BasicCardGame.ShuffleNewCards()
                        game.ShuffleNewCards();

                        CardsInDeck = MAXCARDS;
                        lblCardsInDeck.Text = CardsInDeck.ToString();

                        CardsInDealersHand = 0;
                        lblCardsInDealersHand.Text = CardsInDealersHand.ToString();

                        // Clear the table, set up the UI for playing a game
                        ClearTable();
                        SetUpGameInPlay();

                        if (tFlag)
                        {
                            lblNew.Show();
                            lblNew.Text = "Shuffled Deck";
                            lblNew.ForeColor = Color.Gold;
                            lblDeck.Show();
                            lblDealer.Show();
                            lblCardsInDeck.Show();
                            lblCardsInDealersHand.Show();
                        }
                        else
                        {
                            lblNew.Hide();
                            lblDeck.Hide();
                            lblDealer.Hide();
                            lblCardsInDeck.Hide();
                            lblCardsInDealersHand.Hide();
                        }

                        FlagShuffleCards = true;

                        // Refers to BasicCardGame.DealNewGame()
                        game.DealNewGame(NumberOfCards);

                        _x1 = deckCard1.Location.X;
                        _y1 = deckCard1.Location.Y;
                        deckCard1.Show();
                        deckCard1.BringToFront();

                        DeckCardPB.Location = new Point(_x1 + 0, _y1);
                        DeckCardPB.Show();
                        DeckCardPB.BringToFront();

                        // Send over number of card to deal out, ie total of 52 cards
                        for (int i = 0; i <= NumberOfCards; i++)
                        {
                            UpdateUIDealerCards(i);
                            if (i == NumberOfCards)
                            {
                                NewDeckBtn.Enabled = true;
                                FlagShuffleCards = true;
                            }
                        }
                        DeckCardPB.Location = deckCard1.Location;
                        DeckCardPB.Hide();
                        deckCard1.Hide();
                        dealButton.Enabled = true;
                    }
                    else
                    {
                        lblNew.Hide();
                        FlagShuffleCards = true;
                    }
                }
                else
                {
                    // Start new game
                    //MessageBox.Show("Click on New Deck!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("File is missing or corrupted!" + " " + ex.ToString(), "Error1", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
            }
        }

        private void ShowHandsBtn_Click(object sender, EventArgs e)
        {
            // Show the face cards, hide backs
            List<Card> dcards = game.Dealer.Hand.Cards;

            for (int i = 0; i < dcards.Count; i++)
            {
                dcards[i].IsCardUp = true;
                LoadCard(dealerCards[i], dcards[i]);

                dealerCards[i].Show();
                dealerCards[i].BringToFront();
            }
        }

        private void HideHandsBtn_Click(object sender, EventArgs e)
        {
            // Hide the face cards, show backs
            List<Card> dcards = game.Dealer.Hand.Cards;

            for (int i = 0; i < dcards.Count; i++)
            {
                dcards[i].IsCardUp = false;
                LoadCard(dealerCards[i], dcards[i]);

                dealerCards[i].Show();
                dealerCards[i].BringToFront();
            }
        }

        private void NewDeckBtn_Click(object sender, EventArgs e)
        {
            // Ask to start new game - dialog box with two buttons: yes and no.
            DialogResult result1 = MessageBox.Show("Would you like to deal new deck of cards?", "Deal New Deck of Cards", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result1 == DialogResult.Yes)
            {
                // Start shuffle cards
                NewGameStarted = true;
                FlagShuffleCards = false;

                // Clear the table, set up the UI for playing a game
                ClearTable();
                SetUpGameInPlay();

                // Refers to BasicCardGame.ShowNewDeck()
                game.ShowNewDeck();

                CardsInDeck = MAXCARDS;
                lblCardsInDeck.Text = CardsInDeck.ToString();

                CardsInDealersHand = 0;
                lblCardsInDealersHand.Text = CardsInDealersHand.ToString();

                if (tFlag)
                {
                    lblNew.Show();
                    lblNew.Text = "New Deck";
                    lblNew.ForeColor = Color.Gold;

                    lblDeck.Show();
                    lblDealer.Show();
                    lblCardsInDeck.Show();
                    lblCardsInDealersHand.Show();
                }
                else
                {
                    lblNew.Hide();
                    lblDeck.Hide();
                    lblDealer.Hide();
                    lblCardsInDeck.Hide();
                    lblCardsInDealersHand.Hide();
                }

                game.DealNewGame(NumberOfCards);

                _x1 = deckCard1.Location.X;
                _y1 = deckCard1.Location.Y;

                deckCard1.Show();
                deckCard1.BringToFront();

                DeckCardPB.Location = new Point(_x1 + 0, _y1);
                DeckCardPB.Show();
                DeckCardPB.BringToFront();

                // Send over number of card to deal out, ie total of 52 cards
                for (int i = 0; i <= NumberOfCards; i++)
                {
                    UpdateUIDealerCards(i);
                    if (i == NumberOfCards)
                    {
                        NewDeckBtn.Enabled = true;
                        FlagShuffleCards = true;
                    }
                }

                DeckCardPB.Location = deckCard1.Location;
                DeckCardPB.Hide();
                deckCard1.Hide();
                dealButton.Enabled = true;
                // End shuffle cards
            }
            else
            {
                // Start new game
                //NewGameStarted = true;
            }
        }

        // Start animate cards
        private void UpdateUIDealerCards(int cardNum)
        {
            // Start animation to deal the card

            // Save the original location of the PicBoxDealerCard (card to animate)
            _deckstartPoint = DeckCardPB.Location;

            List<Card> dcards = game.Dealer.Hand.Cards;

            // Start animation
            AnimationTimerDealer.Start();

            // DealerCard
            DealerCardLocationX = dealerCards[cardNum].Location.X;
            DealerCardLocationY = dealerCards[cardNum].Location.Y;

            AnimationDone = false;
            if (AnimationDone == false)
            {
                LoadCard(dealerCards[cardNum], dcards[cardNum]);
                AnimateDealerCard(cardNum);
            }
            AnimationTimerDealer.Stop();
            AnimationTimerDealer.Enabled = false;
            //MessageBox.Show("1Dealer cardNum " + cardNum, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void AnimateDealerCard(int cardIndex)
        {
            CardsInDeck--;
            CardsInDealersHand++;

            lblCardsInDeck.Text = CardsInDeck.ToString();
            lblCardsInDealersHand.Text = CardsInDealersHand.ToString();

            PlaySoundFiles("card_drop");

            DeckCardPB.BringToFront();
            DeckCardPB.Location = _deckstartPoint;
            _DealerCardIndex = cardIndex;
            AnimationTimerDealer.Enabled = true;

            while (AnimationTimerDealer.Enabled == true)
                // Wait for animation to finish
                Application.DoEvents();
        }

        private void AnimateDealerCardTimer_Tick(object sender, EventArgs e)
        {
            // Move card one frame per timer tick

            if (_Increment <= 500)
            {
                // Card is dealt, need to use as endpoints for card locations.
                _a = DealerCardLocationX;
                _b = DealerCardLocationY;
            }
            _x = (_a - _deckstartPoint.X) * _Increment / 500 + _deckstartPoint.X;
            _y = (_b - _deckstartPoint.Y) * _Increment / 500 + _deckstartPoint.Y;

            //MessageBox.Show("_deckstartPoint x and y " + _deckstartPoint.X + " " + _deckstartPoint.Y, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //MessageBox.Show("1_x and _y and _Increment " + _x + " " + _y + " " + _Increment, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            DeckCardPB.Location = new Point(_x, _y);
            DeckCardPB.Show();
            DeckCardPB.BringToFront();

            if (_y <= (_b + 1) && AnimationDone == false)
            {
                // End animation before top of card is reached
                AnimationDone = true;
                //MessageBox.Show("2_x and _a and _Increment " + _x + " " + _a + " " + _Increment, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (_Increment > 500 || AnimationDone == true)
            {
                AnimationTimerDealer.Enabled = false;
                if (_DealerCardIndex >= 0)
                {
                    dealerCards[_DealerCardIndex].Show();
                    dealerCards[_DealerCardIndex].BringToFront();
                }
                DeckCardPB.Location = _deckstartPoint;
                _Increment = 0;
            }
            // How fast to deal Dealer cards, 75
            _Increment += DealSpeed;
        }

        // End animate cards

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            // Exit application
            Application.Exit();
        }

        private void ClearTableBtn_Click(object sender, EventArgs e)
        {
            // Call to clear the table of cards
            ClearTable();
        }

        private void ClearTable()
        {
            // Clear the table of cards

            for (int i = 0; i <= NumberOfCards; i++)
            {
                dealerCards[i].Image = null;
                dealerCards[i].Visible = false;
            }

            HideHandsBtn.Enabled = false;
            ShowHandsBtn.Enabled = false;
            clearTableButton.Enabled = false;
            dealButton.Enabled = false;

            lblNew.Hide();
            lblDeck.Hide();
            lblDealer.Hide();
            lblCardsInDeck.Hide();
            lblCardsInDealersHand.Hide();

            lblNew.Text = "";

        }

        private void Delay(int miliseconds_to_sleep)
        {
            // Set delay, 1500 ms = 1.5 sec
            System.Threading.Thread.Sleep(miliseconds_to_sleep);
        }

        private void PlaySoundFiles(string tString)
        {
            // Play sound files
            if (NSound == 1)
            {
                // Sound files in resources - card_drop, defeat, hooray, shuffling_cards, SoundPlayer player = new SoundPlayer(Properties.Resources.hooray);
                switch (tString)
                {
                    case "shuffling_cards":
                        // If using sound files in resoures
                        //SoundPlayer player1 = new SoundPlayer(Properties.Resources.chord);
                        //player1.Play();
                        //player1.Dispose();

                        // Set location of the .wav file
                        SoundFileLocation = SoundFileFolder + "shuffling_cards.wav";
                        SoundPlayer player1 = new SoundPlayer(SoundFileLocation.ToString());
                        player1.Play();
                        player1.Dispose();
                        break;
                    case "card_drop":
                        SoundFileLocation = SoundFileFolder + "card_drop.wav";
                        SoundPlayer player2 = new SoundPlayer(SoundFileLocation.ToString());
                        player2.Play();
                        player2.Dispose();
                        break;
                    default:
                        //SoundPlayer player = new SoundPlayer(Properties.Resources.card_drop);
                        //player.Play();
                        //player.Dispose();
                        break;
                }   // End switch
            }   // End if NSound
        }   // End PlaySoundFiles

        private void LoadCard(PictureBox pb, Card c)
        {
            // Load the corresponding card image from file

            try
            {
                StringBuilder image = new StringBuilder();

                switch (c.Suit)
                {
                    case Suit.Diamonds:
                        image.Append("d");
                        break;
                    case Suit.Hearts:
                        image.Append("h");
                        break;
                    case Suit.Spades:
                        image.Append("s");
                        break;
                    case Suit.Clubs:
                        image.Append("c");
                        break;
                }

                switch (c.FaceVal)
                {
                    case FaceValue.Ace:
                        image.Append("1");
                        break;
                    case FaceValue.King:
                        image.Append("k");
                        break;
                    case FaceValue.Queen:
                        image.Append("q");
                        break;
                    case FaceValue.Jack:
                        image.Append("j");
                        break;
                    case FaceValue.Ten:
                        image.Append("10");
                        break;
                    case FaceValue.Nine:
                        image.Append("9");
                        break;
                    case FaceValue.Eight:
                        image.Append("8");
                        break;
                    case FaceValue.Seven:
                        image.Append("7");
                        break;
                    case FaceValue.Six:
                        image.Append("6");
                        break;
                    case FaceValue.Five:
                        image.Append("5");
                        break;
                    case FaceValue.Four:
                        image.Append("4");
                        break;
                    case FaceValue.Three:
                        image.Append("3");
                        break;
                    case FaceValue.Two:
                        image.Append("2");
                        break;
                }

                image.Append(Properties.Settings.Default.CardGameImageExtension);
                string cardGameImagePath = Properties.Settings.Default.CardGameImagePath;
                string cardGameImageSkinPath = Properties.Settings.Default.CardGameImageSkinPath;
                image.Insert(0, cardGameImagePath);
                // Check to see if the card should be faced down or up;
                if (!c.IsCardUp)
                    image.Replace(image.ToString(), cardGameImageSkinPath);

                pb.Image = new Bitmap(image.ToString());
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Card images are not loading correctly.  Make sure all card images are in the right location.");
            }
        }

        private void LoadCardSkinImages()
        {
            // Load the Deck Card Back Image

            try
            {
                // Load the card skin image from file
                Image cardSkin = Image.FromFile(Properties.Settings.Default.CardGameImageSkinPath);
                deckCard1.Image = cardSkin;
                DeckCardPB.Image = cardSkin;
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Card skin images are not loading correctly.  Make sure the card skin images are in the correct location.");
            }
            dealerCards = new PictureBox[]
            {
                dealerCard1, dealerCard2, dealerCard3, dealerCard4, dealerCard5, dealerCard6, dealerCard7, dealerCard8, dealerCard9, dealerCard10, dealerCard11, dealerCard12, dealerCard13,
                dealerCard14, dealerCard15, dealerCard16, dealerCard17, dealerCard18, dealerCard19, dealerCard20, dealerCard21, dealerCard22, dealerCard23, dealerCard24, dealerCard25, dealerCard26,
                dealerCard27, dealerCard28, dealerCard29, dealerCard30, dealerCard31, dealerCard32, dealerCard33, dealerCard34, dealerCard35, dealerCard36, dealerCard37, dealerCard38, dealerCard39,
                dealerCard40, dealerCard41, dealerCard42, dealerCard43, dealerCard44, dealerCard45, dealerCard46, dealerCard47, dealerCard48, dealerCard49, dealerCard50, dealerCard51, dealerCard52
            };
        }

        private void SetUpGameInPlay()
        {
            clearTableButton.Enabled = true;
            ShowHandsBtn.Enabled = true;
            HideHandsBtn.Enabled = true;
            dealButton.Enabled = false;
            if (firstTurn)
                ShowHandsBtn.Enabled = true;
        }

        private void SetUpNewGame()
        {
            dealButton.Enabled = true;
            ShowHandsBtn.Enabled = false;
            HideHandsBtn.Enabled = false;
            clearTableButton.Enabled = false;
            // Display the ShowHands Button
            firstTurn = true;
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CelticEgyptianRatscrewKata
{
    public class Cards : IEnumerable<Card>
    {
        private readonly List<Card> _cards;

        public Cards(IEnumerable<Card> cards)
        {
            _cards = new List<Card>(cards);
        }

        public void AddToTop(Card card)
        {
            _cards.Insert(0, card);
        }

        public void AddToBottom(Card card)
        {
            _cards.Insert(_cards.Count, card);
        }

        public Card Pop()
        {
            var first = _cards.First();
            _cards.RemoveAt(0);
            return first;
        }

        public Card CardAt(int i)
        {
            return _cards.ElementAt(i);
        }

        public Card TopCard
        {
            get { return _cards.First(); }
        }

        public void RemoveCardAt(int i)
        {
            _cards.RemoveAt(i);
        }

        public bool HasCards
        {
            get { return _cards.Count > 0; } 
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static Cards Empty()
        {
            return With();
        }

        public static Cards With(Cards cards)
        {
            return With(cards.ToArray());
        }

        public static Cards With(params Card[] cards)
        {
            return new Cards(cards);
        }

        public override string ToString()
        {
            var output = "";

            foreach (var card in _cards)
            {
                if (!output.Equals("")) output += ", ";
                output += card;
            }

            return output;
        }

        public static Cards CreateFullDeck()
        {
            return Cards.With(
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Clubs, Rank.Two),
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Clubs, Rank.Four),
                new Card(Suit.Clubs, Rank.Five),
                new Card(Suit.Clubs, Rank.Six),
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Clubs, Rank.Nine),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Clubs, Rank.Jack),
                new Card(Suit.Clubs, Rank.Queen),
                new Card(Suit.Clubs, Rank.King),
                
                new Card(Suit.Diamonds, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Two),
                new Card(Suit.Diamonds, Rank.Three),
                new Card(Suit.Diamonds, Rank.Four),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Nine),
                new Card(Suit.Diamonds, Rank.Ten),
                new Card(Suit.Diamonds, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Queen),
                new Card(Suit.Diamonds, Rank.King),
                
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Hearts, Rank.Two),
                new Card(Suit.Hearts, Rank.Three),
                new Card(Suit.Hearts, Rank.Four),
                new Card(Suit.Hearts, Rank.Five),
                new Card(Suit.Hearts, Rank.Six),
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Hearts, Rank.Eight),
                new Card(Suit.Hearts, Rank.Nine),
                new Card(Suit.Hearts, Rank.Ten),
                new Card(Suit.Hearts, Rank.Jack),
                new Card(Suit.Hearts, Rank.Queen),
                new Card(Suit.Hearts, Rank.King),
                
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Spades, Rank.Two),
                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Spades, Rank.Four),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Spades, Rank.Six),
                new Card(Suit.Spades, Rank.Seven),
                new Card(Suit.Spades, Rank.Eight),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.King)
                );
        }
    }
}

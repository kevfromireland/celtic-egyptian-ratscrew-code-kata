using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CelticEgyptianRatscrewKata.Game
{
    /// <summary>
    /// Represents the state of the game at any point.
    /// </summary>
    public class GameState : IGameState
    {
        public enum SnapPenaltyState
        {
            PenaltyApplied,
            PenaltyNotApplied
        }

        private readonly Cards _stack;
        private readonly IDictionary<string, Cards> _decks;
        private readonly IDictionary<string, SnapPenaltyState> _playerSnapPenaltyState;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GameState()
            : this(Cards.Empty(), new Dictionary<string, Cards>(), new Dictionary<string, SnapPenaltyState>()) {}

        /// <summary>
        /// Constructor to allow setting the central stack.
        /// </summary>
        public GameState(Cards stack, IDictionary<string, Cards> decks, Dictionary<string, SnapPenaltyState> playerSnapPenaltyState)
        {
            _stack = stack;
            _decks = decks;
            _playerSnapPenaltyState = playerSnapPenaltyState;
        }

        public Cards Stack { get {return new Cards(_stack);} }

        public string CurrentPlayer { get; set; }

        /// <summary>
        /// Add the given player to the game with the given deck.
        /// </summary>
        /// <exception cref="ArgumentException">If the given player already exists</exception>
        public void AddPlayer(string playerId, Cards deck)
        {
            if (_decks.ContainsKey(playerId)) throw new ArgumentException("Can't add the same player twice");
            _decks.Add(playerId, deck);
            _playerSnapPenaltyState.Add(playerId, SnapPenaltyState.PenaltyNotApplied);
        }

        /// <summary>
        /// Play the top card of the given player's deck.
        /// </summary>
        public Card PlayCard(string playerId)
        {
            if (!_decks.ContainsKey(playerId)) throw new ArgumentException("The selected player doesn't exist");
            if (!_decks[playerId].Any()) throw new ArgumentException("The selected player doesn't have any cards left");

            var topCard = _decks[playerId].Pop();
            _stack.AddToTop(topCard);
            return topCard;
        }

        public Card AddCardToBottom(string playerId)
        {
            if (!_decks.ContainsKey(playerId)) throw new ArgumentException("The selected player does not exist");
            if (!_decks[playerId].Any()) throw new ArgumentException("The selected player doesn't have any cards left");

            var topCard = _decks[playerId].Pop();
            _stack.AddToBottom(topCard);
            return topCard;
        }

        /// <summary>
        /// Wins the stack for the given player.
        /// </summary>
        public void WinStack(string playerId)
        {
            if (!_decks.ContainsKey(playerId)) throw new ArgumentException("The selected player doesn't exist");

            foreach (var card in _stack.Reverse())
            {
                _decks[playerId].AddToBottom(card);
            }

            ClearStack();
        }

        private void ClearStack()
        {
            while (_stack.HasCards)
            {
                _stack.RemoveCardAt(0);
            }
        }

        public bool HasCards(string playerId)
        {
            if (!_decks.ContainsKey(playerId)) throw new ArgumentException("The selected player doesn't exist");
            return _decks[playerId].Any();
        }

        public void ApplyPenalty(string playerId)
        {
            if (!_playerSnapPenaltyState.ContainsKey(playerId)) throw new ArgumentException();

            _playerSnapPenaltyState[playerId] = SnapPenaltyState.PenaltyApplied;
        }

        public bool HasPenaltyApplied(string playerId)
        {
            if (!_playerSnapPenaltyState.ContainsKey(playerId)) throw new ArgumentException();

            return _playerSnapPenaltyState[playerId] == SnapPenaltyState.PenaltyApplied;
        }

        public int NumberOfCards(string playerId)
        {
            if (!_decks.ContainsKey(playerId)) throw new ArgumentException("The selected player doesn't exist");
            return _decks[playerId].Count();
        }

        public void Clear()
        {
            ClearStack();
            _decks.Clear();
        }
    }
}

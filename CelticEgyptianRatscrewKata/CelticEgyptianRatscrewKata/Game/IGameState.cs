﻿using System;

namespace CelticEgyptianRatscrewKata.Game
{
    public interface IGameState
    {
        /// <summary>
        /// Gets a copy of the current stack of cards.
        /// </summary>
        Cards Stack { get; }

        string CurrentPlayer { get; set; }

        /// <summary>
        /// Add the given player to the game with the given deck.
        /// </summary>
        /// <exception cref="ArgumentException">If the given player already exists</exception>
        void AddPlayer(string playerId, Cards deck);

        /// <summary>
        /// Play the top card of the given player's deck.
        /// </summary>
        Card PlayCard(string playerId);

        /// <summary>
        /// Moves the top card of the given player's deck to the bottom of the stack
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Card AddCardToBottom(string playerId);

        /// <summary>
        /// Wins the stack for the given player.
        /// </summary>
        void WinStack(string playerId);

        /// <summary>
        /// Returns true if the given player has any cards in their hand.
        /// </summary>
        bool HasCards(string playerId);

        /// <summary>
        /// Resets the game state back to its default values.
        /// </summary>
        void Clear();

        int NumberOfCards(string name);
        void ApplyPenalty(string playerId);
        bool HasPenaltyApplied(string playerId);
    }
}
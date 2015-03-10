using System;
using System.Collections.Generic;
using System.Linq;
using CelticEgyptianRatscrewKata.GameSetup;
using CelticEgyptianRatscrewKata.SnapRules;

namespace CelticEgyptianRatscrewKata.Game
{
    /// <summary>
    /// Controls a game of Celtic Egyptian Ratscrew.
    /// </summary>
    public class GameController : IGameController
    {
        private readonly ISnapValidator _snapValidator;
        private readonly IDealer _dealer;
        private readonly IShuffler _shuffler;
        private readonly IList<IPlayer> _players;
        private readonly IGameState _gameState;
        private Dictionary<string, string> _nextPlayerMapping;

        public GameController(IGameState gameState, ISnapValidator snapValidator, IDealer dealer, IShuffler shuffler)
        {
            _nextPlayerMapping = new Dictionary<string, string>();
            _players = new List<IPlayer>();
            _gameState = gameState;
            _snapValidator = snapValidator;
            _dealer = dealer;
            _shuffler = shuffler;
        }

        public IEnumerable<IPlayer> Players
        {
            get { return _players; }
        }

        public int StackSize
        {
            get { return _gameState.Stack.Count(); }
        }

        public Card TopOfStack
        {
            get { return _gameState.Stack.FirstOrDefault(); }
        }

        public int NumberOfCards(IPlayer player)
        {
            return _gameState.NumberOfCards(player.Name);
        }

        public bool AddPlayer(IPlayer player)
        {
            if (Players.Any(x => x.Name == player.Name)) return false;

            _players.Add(player);
            _gameState.AddPlayer(player.Name, Cards.Empty());
            return true;
        }

        public PlayCardResult PlayCard(IPlayer player)
        {
            if (!_gameState.HasCards(player.Name))
                return PlayCardResult.NoCard();

            if (_gameState.CurrentPlayer != player.Name)
            {
                _gameState.ApplyPenalty(player.Name);
                return PlayCardResult.OutOfTurn(_gameState.AddCardToBottom(player.Name));
            }

            var playCardResult = PlayCardResult.Valid(_gameState.PlayCard(player.Name));

            _gameState.CurrentPlayer = _nextPlayerMapping[player.Name];

            return playCardResult;
        }

        public bool AttemptSnap(IPlayer player)
        {
            AddPlayer(player);

            if (_snapValidator.CanSnap(_gameState.Stack))
            {
                _gameState.WinStack(player.Name);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Starts a game with the currently added players
        /// </summary>
        public void StartGame(Cards deck)
        {
            _gameState.Clear();

            var shuffledDeck = _shuffler.Shuffle(deck);
            var decks = _dealer.Deal(_players.Count, shuffledDeck);
            for (var i = 0; i < decks.Count; i++)
            {
                _gameState.AddPlayer(_players[i].Name, decks[i]);
            }

            _nextPlayerMapping = _players
                .Zip(_players.Skip(1), Tuple.Create)
                .Concat(new[] { Tuple.Create(_players.Last(), _players.First()) })
                .ToDictionary(x => x.Item1.Name, x => x.Item2.Name);

            _gameState.CurrentPlayer = _players.First().Name;
        }

        public bool TryGetWinner(out IPlayer winner)
        {
            var playersWithCards = _players.Where(p => _gameState.HasCards(p.Name)).ToList();

            if (!_gameState.Stack.Any() && playersWithCards.Count() == 1)
            {
                winner = playersWithCards.Single();
                return true;
            }

            winner = null;
            return false;
        }
    }
}

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
        private readonly IPenalties _penalties;
        private readonly IPlayerSequence _playerSequence;
        private readonly ICalloutSequence _calloutSequence;

        public GameController(IGameState gameState, ISnapValidator snapValidator, IDealer dealer, IShuffler shuffler, IPenalties penalties, IPlayerSequence playerSequence, ICalloutSequence calloutSequence)
        {
            _players = new List<IPlayer>();
            _gameState = gameState;
            _snapValidator = snapValidator;
            _dealer = dealer;
            _shuffler = shuffler;
            _penalties = penalties;
            _playerSequence = playerSequence;
            _calloutSequence = calloutSequence;
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
        }

        public bool AddPlayer(IPlayer player)
        {
            if (Players.Any(x => x.Name == player.Name)) return false;

            _players.Add(player);
            _gameState.AddPlayer(player.Name, Cards.Empty());

            _penalties.AddPlayer(player);
            _playerSequence.AddPlayer(player.Name);

            return true;
        }

        public Card PlayCard(IPlayer player)
        {
            if (!_playerSequence.IsCurrentPlayer(player.Name))
            {
                return ExecuteAttemptToPlayOutOfTurn(player);
            }
            if (_gameState.HasCards(player.Name))
            {
                return ExecutePlayCard(player);
            }
            return ExecuteSkipTurn();
        }

        public bool AttemptSnap(IPlayer player)
        {
            if (_penalties.HasPenalty(player))
            {
                return ExecuteNoSnap();
            }
            if (_snapValidator.CanSnap(_gameState.Stack))
            {
                return ExecuteValidSnap(player);
            }
            return ExecuteInvalidSnap(player);
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

        private bool ExecuteInvalidSnap(IPlayer player)
        {
            _penalties.GivePenalty(player);
            return false;
        }

        private bool ExecuteValidSnap(IPlayer player)
        {
            _gameState.WinStack(player.Name);
            _penalties.ClearPenalties();
            return true;
        }

        private static bool ExecuteNoSnap()
        {
            return false;
        }

        private static Card ExecuteSkipTurn()
        {
            return null;
        }

        private Card ExecutePlayCard(IPlayer player)
        {
            _playerSequence.AdvanceToNextPlayer();
            var playedCard = _gameState.PlayCard(player.Name);
            _calloutSequence.Callout();
            return playedCard;
        }

        private Card ExecuteAttemptToPlayOutOfTurn(IPlayer player)
        {
            _penalties.GivePenalty(player);
            return null;
        }
    }
}

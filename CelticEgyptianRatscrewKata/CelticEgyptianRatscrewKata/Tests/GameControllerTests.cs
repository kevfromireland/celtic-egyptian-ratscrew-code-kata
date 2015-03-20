using CelticEgyptianRatscrewKata.Game;
using CelticEgyptianRatscrewKata.GameSetup;
using CelticEgyptianRatscrewKata.SnapRules;
using NSubstitute;
using NUnit.Framework;

namespace CelticEgyptianRatscrewKata.Tests
{
    public class GameControllerTests
    {
        [Test]
        public void ShouldApplyPenaltyIfPlayerPlaysOutOfTurn()
        {
            // Arrange
            var playerSequence = Substitute.For<IPlayerSequence>();
            playerSequence.IsCurrentPlayer(Arg.Any<string>()).Returns(false);

            var penalties = Substitute.For<IPenalties>();

            var gameController = new GameController(
                new GameState(), 
                Substitute.For<ISnapValidator>(), 
                new Dealer(), 
                new NoneShufflingShuffler(), 
                penalties, 
                playerSequence, new CalloutSequence());

            var playerA = new Player("playerA");
            gameController.AddPlayer(playerA);

            // Act
            gameController.StartGame(CreateNewSimpleDeck());
            gameController.PlayCard(playerA);
            
            // Assert
            penalties.Received().GivePenalty(playerA);

        }

        [Test]
        public void RedRouteWinnerAfterSomeRoundsOfPlay()
        {
            // Arrange
            var gameController = CreateGameController();
            var playerA = new Player("playerA");
            var playerB = new Player("playerB");
            var playerC = new Player("playerC");
            var playerD = new Player("playerD");
            var deck = CreateNewSimpleDeck();

            // Act
            gameController.AddPlayer(playerA);
            gameController.AddPlayer(playerB);
            gameController.AddPlayer(playerC);
            gameController.AddPlayer(playerD);
            gameController.StartGame(deck);

            gameController.PlayCard(playerA);
            gameController.PlayCard(playerB);
            gameController.PlayCard(playerC);
            gameController.PlayCard(playerD);
            gameController.PlayCard(playerA);
            gameController.PlayCard(playerB);
            gameController.AttemptSnap(playerC);

            gameController.PlayCard(playerC);
            gameController.PlayCard(playerD);
            gameController.PlayCard(playerA);
            gameController.PlayCard(playerB);
            gameController.AttemptSnap(playerC);

            // Assert
            IPlayer winner;
            var hasWinner = gameController.TryGetWinner(out winner);
            Assert.True(hasWinner);
            Assert.That(winner.Name, Is.EqualTo(playerC.Name));
        }

        [Test]
        public void RedRouteNoWinnerAfterSomeRoundsOfPlay()
        {
            // Arrange
            var gameController = CreateGameController();
            var playerA = new Player("playerA");
            var playerB = new Player("playerB");
            var playerC = new Player("playerC");
            var playerD = new Player("playerD");
            var deck = CreateNewSimpleDeck();

            // Act
            gameController.AddPlayer(playerA);
            gameController.AddPlayer(playerB);
            gameController.AddPlayer(playerC);
            gameController.AddPlayer(playerD);
            gameController.StartGame(deck);

            gameController.PlayCard(playerA);
            gameController.PlayCard(playerB);
            gameController.PlayCard(playerC);
            gameController.PlayCard(playerD);
            gameController.PlayCard(playerA);
            gameController.PlayCard(playerB);
            gameController.AttemptSnap(playerC);

            // Assert
            IPlayer potentialWinner;
            var hasWinner = gameController.TryGetWinner(out potentialWinner);
            Assert.False(hasWinner);
        }

        private static GameController CreateGameController()
        {
            var gameState = new GameState();
            var completeSnapValidator = CreateCompleteSnapValidator();
            var dealer = new Dealer();
            var noneShufflingShuffler = new NoneShufflingShuffler();

            return new GameController(gameState, completeSnapValidator, dealer, noneShufflingShuffler, Substitute.For<IPenalties>(), new LaxPlayerSequence(), new CalloutSequence());
        }

        private static ISnapValidator CreateCompleteSnapValidator()
        {
            var rules = new ISnapRule[]
                        {
                            new DarkQueenSnapRule(),
                            new SandwichSnapRule(),
                            new StandardSnapRule()
                        };
            return new SnapValidator(rules);
        }

        public static Cards CreateNewSimpleDeck()
        {
            return Cards.With(
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Diamonds, Rank.Three),
                new Card(Suit.Clubs, Rank.Five),
                new Card(Suit.Clubs, Rank.Four),
                new Card(Suit.Clubs, Rank.Six),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Clubs, Rank.Nine)
                );
        }
    }

    public class NoneShufflingShuffler : IShuffler
    {
        public Cards Shuffle(Cards deck)
        {
            return new Cards(deck);
        }
    }

    public class LaxPlayerSequence : IPlayerSequence
    {
        public void AddPlayer(string name)
        {
        }

        public void AdvanceToNextPlayer()
        {
        }

        public bool IsCurrentPlayer(string name)
        {
            return true;
        }
    }
}
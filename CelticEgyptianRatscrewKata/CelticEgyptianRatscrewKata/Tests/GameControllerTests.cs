using System.Collections.Generic;
using System.Linq;
using CelticEgyptianRatscrewKata.Game;
using CelticEgyptianRatscrewKata.GameSetup;
using CelticEgyptianRatscrewKata.SnapRules;
using Moq;
using NSubstitute;
using NUnit.Framework;

namespace CelticEgyptianRatscrewKata.Tests
{
    public class GameControllerTests
    {
        [Test]
        public void CardsByPlayersOutOfTurnGoToBottomOfTheStack()
        {
            var gameState = new GameState();
            var gameController = CreateGameController(gameState);
            var playerA = new Player("A");
            var playerB = new Player("B");
            var deck = CreateNewSimpleDeck();

            gameController.AddPlayer(playerA);
            gameController.AddPlayer(playerB);
            gameController.StartGame(deck);

            //Act
            gameController.PlayCard(playerA);
            var resultAfterSecondCard = gameController.PlayCard(playerA);

            //Expectation
            Assert.That(resultAfterSecondCard.Validity, Is.EqualTo(PlayCardResultValidity.PlayedOutOfTurn));
            Assert.That(resultAfterSecondCard.PlayedCard, Is.EqualTo(gameState.Stack.Last()));
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

        private static GameController CreateGameController(IGameState gameState = null)
        {
            var completeSnapValidator = CreateCompleteSnapValidator();
            var dealer = new Dealer();
            var noneShufflingShuffler = new NoneShufflingShuffler();

            return new GameController(gameState ?? new GameState(), completeSnapValidator, dealer, noneShufflingShuffler);
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
}
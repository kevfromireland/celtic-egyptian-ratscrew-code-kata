using System.Collections.Generic;
using CelticEgyptianRatscrewKata.Game;
using CelticEgyptianRatscrewKata.GameSetup;
using CelticEgyptianRatscrewKata.SnapRules;
using NSubstitute;
using NUnit.Framework;

namespace CelticEgyptianRatscrewKata.Tests
{
    [TestFixture]
    public class EndToEndPenaltyTests
    {
        [Test]
        public void SnappingWhenOnePlayerHasAPenaltyShouldFailIfSnapValid()
        {
            // ARRANGE
            var player1 = new Player("p1");
            var player2 = new Player("p2");
            var snapValidator = Substitute.For<ISnapValidator>();
            snapValidator.CanSnap(Arg.Any<Cards>()).Returns(false, true);

            var gameController = new GameController(new GameState(), snapValidator, Substitute.For<IDealer>(), Substitute.For<IShuffler>(), new Penalties(), Substitute.For<IPlayerSequence>(), new CalloutSequence());
            gameController.AddPlayer(player1);
            gameController.AddPlayer(player2);

            // ACT
            // Get a penalty
            gameController.AttemptSnap(player1);
            // Try a `valid` snap
            var success = gameController.AttemptSnap(player1);

            // ASSERT
            Assert.IsFalse(success, "The snap should not succeed");
        }

        [Test]
        public void PenaltyIsRemovedAfterSuccessfulSnap()
        {
            // ARRANGE
            var player1 = new Player("p1");
            var player2 = new Player("p2");
            var snapValidator = Substitute.For<ISnapValidator>();
            snapValidator.CanSnap(Arg.Any<Cards>()).Returns(false, true);

            var gameController = new GameController(new GameState(), snapValidator, Substitute.For<IDealer>(), Substitute.For<IShuffler>(), new Penalties(), Substitute.For<IPlayerSequence>(), new CalloutSequence());
            gameController.AddPlayer(player1);
            gameController.AddPlayer(player2);

            // ACT
            // Get a penalty
            gameController.AttemptSnap(player1);
            var snapped = gameController.AttemptSnap(player2);
            // Try a `valid` snap, after penalty removed
            var success = gameController.AttemptSnap(player1);

            // ASSERT
            Assert.IsTrue(snapped, "The snap should succeed, this is ensuring player 2 did not get a penalty");
            Assert.IsTrue(success, "The snap should succeed");
        }

        [Test]
        public void PenaltyIsRemovedWhenEveryoneHasAPenalty()
        {
            // ARRANGE
            var player1 = new Player("p1");
            var player2 = new Player("p2");
            var snapValidator = Substitute.For<ISnapValidator>();
            snapValidator.CanSnap(Arg.Any<Cards>()).Returns(false, false, true);

            var gameController = new GameController(new GameState(), snapValidator, Substitute.For<IDealer>(), Substitute.For<IShuffler>(), new Penalties(), Substitute.For<IPlayerSequence>(), new CalloutSequence());
            gameController.AddPlayer(player1);
            gameController.AddPlayer(player2);

            // ACT
            // Get a penalty
            gameController.AttemptSnap(player1);
            var snapped = gameController.AttemptSnap(player2);
            // Try a `valid` snap, after penalty removed because deadlock
            var success = gameController.AttemptSnap(player1);

            // ASSERT
            Assert.IsFalse(snapped, "The snap should not succeed, this is ensuring both players have a penalty");
            Assert.IsTrue(success, "The snap should succeed");
        }
    }
}

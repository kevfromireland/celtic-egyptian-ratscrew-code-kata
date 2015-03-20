using System;
using System.Linq;
using CelticEgyptianRatscrewKata.Game;
using NUnit.Framework;

namespace CelticEgyptianRatscrewKata.Tests.SnapRules
{
    [TestFixture]
    public class CallOutSequenceTests
    {
        [Test]
        public void InitialStateCorrect()
        {
            Assert.That(new CalloutSequence().Callout(), Is.EqualTo(Rank.Ace));
        }

        [Test]
        public void CanIncrementOneSpace()
        {
            var calloutSequence = new CalloutSequence();
            var first = calloutSequence.Callout();
            var second = calloutSequence.Callout();

            Assert.That(second, Is.EqualTo(Rank.Two));
        }

        [Test]
        public void CanLoopToAce()
        {
            var calloutSequence = new CalloutSequence();
            var ranks = Enum.GetValues(typeof (Rank)).Cast<Rank>().ToList();

            foreach (var rank in ranks)
            {
                Assert.That(calloutSequence.Callout(), Is.EqualTo(rank));
            }

            Assert.That(calloutSequence.Callout(), Is.EqualTo(Rank.Ace));
        }
    }
}
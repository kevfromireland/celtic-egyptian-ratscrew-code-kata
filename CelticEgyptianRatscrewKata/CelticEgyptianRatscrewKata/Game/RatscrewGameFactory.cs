using System.Linq;
using CelticEgyptianRatscrewKata.GameSetup;
using CelticEgyptianRatscrewKata.SnapRules;

namespace CelticEgyptianRatscrewKata.Game
{
    public class RatscrewGameFactory : IGameFactory
    {
        public IGameController Create(ILog log)
        {
            var calloutSequence = new CalloutSequence();
            var loggedCalloutSequence = new LoggedCalloutSequence(calloutSequence, log);

            ISnapRule[] rules =
            {
                new DarkQueenSnapRule(),
                new SandwichSnapRule(),
                new StandardSnapRule(),
                new CalloutSnapRule(loggedCalloutSequence)
            };

            var penalties = new Penalties();
            var loggedPenalties = new LoggedPenalties(penalties, log);

            var gameController = new GameController(new GameState(), new SnapValidator(rules), new Dealer(), new Shuffler(), loggedPenalties, new PlayerSequence(), loggedCalloutSequence);
            return new LoggedGameController(gameController, log);
        }
    }
}
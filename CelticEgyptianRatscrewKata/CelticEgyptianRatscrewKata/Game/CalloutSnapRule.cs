using CelticEgyptianRatscrewKata.SnapRules;

namespace CelticEgyptianRatscrewKata.Game
{
    public class CalloutSnapRule : ISnapRule
    {
        private readonly ICalloutSequence m_CalloutSequence;

        public CalloutSnapRule(ICalloutSequence calloutSequence)
        {
            m_CalloutSequence = calloutSequence;
        }

        public bool IsSnapValid(Cards cardStack)
        {
            return cardStack.TopCard.Rank == m_CalloutSequence.LastCalledOut;
        }
    }
}
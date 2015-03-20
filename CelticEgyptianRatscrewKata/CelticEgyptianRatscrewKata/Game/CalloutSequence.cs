using System;
using System.Collections.Generic;
using System.Linq;

namespace CelticEgyptianRatscrewKata.Game
{
    public class CalloutSequence : ICalloutSequence
    {
        private readonly List<Rank> m_Ranks;
        private int m_Current;
        private Rank? m_LastCalledOut;

        public CalloutSequence()
        {
            m_Ranks = Enum.GetValues(typeof (Rank)).Cast<Rank>().ToList();
            m_Current = 0;
        }

        public Rank Callout()
        {
            m_LastCalledOut = m_Ranks[m_Current];

            m_Current = ++m_Current%(m_Ranks.Count);

            return m_LastCalledOut.Value;
        }

        public Rank LastCalledOut
        {
            get
            {
                if (!m_LastCalledOut.HasValue)
                    throw new InvalidOperationException("Nothing has been called from this sequence");

                return m_LastCalledOut.Value;
            }
        }
    }
}
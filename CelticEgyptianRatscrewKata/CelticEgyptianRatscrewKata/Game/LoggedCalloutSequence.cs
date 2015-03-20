using System;

namespace CelticEgyptianRatscrewKata.Game
{
    public class LoggedCalloutSequence : ICalloutSequence
    {
        private readonly ICalloutSequence m_CalloutSequence;
        private readonly ILog m_Log;
        private Rank m_LastCalledOut;

        public LoggedCalloutSequence(ICalloutSequence calloutSequence, ILog log)
        {
            m_CalloutSequence = calloutSequence;
            m_Log = log;
        }

        public Rank Callout()
        {
            var callout = m_CalloutSequence.Callout();
            m_Log.Log(String.Format("Called out: {0}", callout));
            return callout;
        }

        public Rank LastCalledOut
        {
            get { return m_LastCalledOut; }
        }
    }
}
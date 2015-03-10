namespace CelticEgyptianRatscrewKata.Game
{
    public class PlayCardResult
    {
        private readonly PlayCardResultValidity m_Validity;
        private readonly Card m_PlayedCard;

        private PlayCardResult(PlayCardResultValidity validity, Card playedCard = null)
        {
            m_Validity = validity;
            m_PlayedCard = playedCard;
        }

        public PlayCardResultValidity Validity
        {
            get { return m_Validity; }
        }

        public Card PlayedCard
        {
            get { return m_PlayedCard; }
        }

        public static PlayCardResult NoCard()
        {
            return new PlayCardResult(PlayCardResultValidity.PlayerHasNoCards);
        }

        public static PlayCardResult Valid(Card playedCard)
        {
            return new PlayCardResult(PlayCardResultValidity.Valid, playedCard);
        }

        public static PlayCardResult OutOfTurn(Card playedCard)
        {
            return new PlayCardResult(PlayCardResultValidity.PlayedOutOfTurn, playedCard);
        }
    }
}
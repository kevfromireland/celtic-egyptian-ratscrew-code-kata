namespace CelticEgyptianRatscrewKata.Game
{
    public interface ICalloutSequence
    {
        Rank Callout();

        Rank LastCalledOut { get; }
    }
}
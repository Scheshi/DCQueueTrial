public interface IBrain : ILegs
{
    void OnTapped();
    void OnServed();
    void OnExpired();
    void OnDestinationReached();
    void OnEmotePlayed();
}

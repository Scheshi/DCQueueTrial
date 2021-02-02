namespace Assets.Scripts.Test
{
    public class VoidBrain : IBrain
    {
        public void OnTapped()
        {
        }

        public void OnServed()
        {
            return;
        }

        public void OnExpired()
        {
            return;
        }

        public void OnDestinationReached()
        {
            return;
        }

        public void OnEmotePlayed()
        {
            return;
        }
    }
}
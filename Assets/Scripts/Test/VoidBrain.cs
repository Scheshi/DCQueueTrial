namespace Assets.Scripts.Test
{
    //Null-object паттерн, на всякий случай, я его юзал для тестов
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
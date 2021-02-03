using UnityEngine;


    public interface IClientController
    {
        Vector3 HomePosition { get; }
        
        void Serve();
        void ExpireNow();
        void OrderStuff();

        void InjectBrain(IBrain brain);
    }
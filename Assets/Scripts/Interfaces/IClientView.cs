using UnityEngine;
using UnityEngine.EventSystems;


    public interface IClientView : IPointerDownHandler
    {
        IBrain Brain { get; }
        IClientController Controller { get; }
        
        void InjectBrain(IBrain brain); 
        
        void InjectController(IClientController controller);
        
    }
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ClientViewProxy : MonoBehaviour, IClientView
{
        
        public abstract void OnPointerDown(PointerEventData eventData);
        public abstract IBrain Brain { get; }
        public abstract IClientController Controller { get; }
        public abstract void InjectBrain(IBrain brain);
        public abstract void InjectController(IClientController controller);
}
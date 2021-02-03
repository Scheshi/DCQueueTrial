using Assets.Scripts.Test;
using UnityEngine;
using UnityEngine.EventSystems;


public class ClientView : MonoBehaviour, IClientView
    {
        private IClientController _controller;
        private IBrain _brain;

        public void InjectController(IClientController controller)
        {
            _controller = controller;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _brain.OnTapped();
        }

        
        
        public IBrain Brain => _brain;
        public IClientController Controller => _controller;

        public void SetBrain(IBrain brain)
        {
            _brain = brain;
        }
    }
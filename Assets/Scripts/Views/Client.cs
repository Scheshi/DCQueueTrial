using Assets.Scripts.Test;
using UnityEngine;
using UnityEngine.EventSystems;


public class Client : ClientViewProxy
    {
        private IClientController _controller;
        private IBrain _brain;

        public override void InjectController(IClientController controller)
        {
            _controller = controller;
        }
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            _brain.OnTapped();
        }

        
        
        public override IBrain Brain => _brain;
        public override IClientController Controller => _controller;

        public override void InjectBrain(IBrain brain)
        {
            _brain = brain;
        }
    }
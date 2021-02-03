using System.Collections;
using UnityEngine;


    public class ClientController : IClientController
    {
        private IBrain _brain;
        private readonly Transform _homeTransform;
        private readonly SpriteMask _orderTimer;
        private readonly MonoBehaviour _monoParser;
        private readonly float _patience;
        private Coroutine _activeOrder;
        private float _timer;
        private int _served;
        

        public ClientController(Transform homeTransform, SpriteMask orderTimer,
            MonoBehaviour mono, ClientStruct clientStruct)
        {
            _homeTransform = homeTransform;
            _orderTimer = orderTimer;
            _monoParser = mono;
            _patience = clientStruct.Patience;
            _served = clientStruct.Served;
            _orderTimer.gameObject.SetActive(false);
        }
        
        public void OrderStuff()
        {
            if(_activeOrder != null) _monoParser.StopCoroutine(_activeOrder);
            _activeOrder = _monoParser.StartCoroutine(WaitForOrder());
        }

        public void SetBrain(IBrain brain)
        {
            if (_brain == null)
            {
                _brain = brain;
            }
        }

        private IEnumerator WaitForOrder()
        {
            _timer = _patience;
            _orderTimer.gameObject.SetActive(true);
            while(_timer > 0) {
                _timer -= Time.deltaTime;
                _orderTimer.alphaCutoff = _timer / _patience;
                yield return null;
            }
            _orderTimer.gameObject.SetActive(false);
            _brain.OnExpired();
            _activeOrder = null;
        }

        public Vector3 HomePosition => _homeTransform.position;

        public void Serve()
        {
            _served ++;
            _orderTimer.gameObject.SetActive(false);
            if(_activeOrder != null) {
                _monoParser.StopCoroutine(_activeOrder);
                _activeOrder = null;
            }
            _brain.OnServed();
        }
        public void ExpireNow() {
            _timer = 0;
        }
    }

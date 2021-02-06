using System;
using System.Collections;
using UnityEngine;


    public class ClientDirectionController : IBrain, ILegs
    {
        //Потому что строки - раковая опухоль шарпов
        private const string HAPPY_EMOTY = "Happy";
        private Mutex _mutex;
        private QueueController _queue;
        private IClientController _clientController;
        private ClientViewProxy _clientView;
        private ILegs _clientLegs;
        private Animator _animator;

        private bool _tapped;
        private bool _served;
        private bool _expired;
        private bool _destinationReached;
        private bool _emotePlayed;
        public ClientDirectionController(Mutex mutex, QueueController queue, ClientViewProxy clientView,
            ILegs legs, Animator animator, IClientController controller)
        {
            _mutex = mutex;
            _queue = queue;
            _clientController = controller;
            _clientView = clientView;
            _clientLegs = legs;
            _animator = animator;
            clientView.StartCoroutine(Idle());
        } 
        
        
    private IEnumerator Idle()
    { 
        _tapped = false;
        _served = false;
        _expired = false;
        _destinationReached = false;
        while (!_tapped)
        {
            yield return null;
        }
        
        GoToQueue();

    }

    private IEnumerator GoToWait()
    {
        _tapped = false;
        _expired = false;
        _served = false;
        _destinationReached = false;
        while (true)
        {
            IEnumerator mutexLock = _mutex.Lock(_clientView);
            if (_queue.IndexOfClient(_clientView) == 0 && !mutexLock.MoveNext())
            {
                _queue.RemoveClientInQueue(_clientView);
                _clientView.StartCoroutine(GoToServer());
                yield break;
            } else if(_tapped) {
                _queue.RemoveClientInQueue(_clientView);
                _clientView.StartCoroutine(GoToBase());
                yield break;
            }
            yield return null;
        }
        
    }

    private IEnumerator GoToServer()
    {
        _tapped = false;
        _expired = false;
        _served = false;
        _destinationReached = false;
        _clientLegs.GoTo(_mutex.transform.position);
        while(true) {
            if(_destinationReached) {
                
                _clientView.StartCoroutine(WaitForServed());
                yield break;
            } else if(_tapped) {
                _clientView.StartCoroutine(GoToBase());
                _mutex.Unlock(_clientView);
                yield break;
            }
            yield return null;
        }
    }
    
    private IEnumerator WaitForServed() {
        _tapped = false;
        _expired = false;
        _served = false;
        _destinationReached = false;
        _clientController.OrderStuff();
        Server server = GameObject.FindObjectOfType<Server>();
        server.CurrentClientView = _clientView;
        while(true) {
            if(_served) {
                _clientView.StartCoroutine(PlayEmote());
                server.CurrentClientView = null;
                yield break;
            } else if(_expired) {
                _clientView.StartCoroutine(GoToBase());
                _mutex.Unlock(_clientView);
                server.CurrentClientView = null;
                yield break;
            } else if(_tapped) {
                _clientController.ExpireNow();
            }
            yield return null;
        }
    }

    private IEnumerator PlayEmote() {
        _emotePlayed = false;
        _animator.SetTrigger(HAPPY_EMOTY);
        while(!_emotePlayed) {
            yield return null;
        }
        _mutex.Unlock(_clientView);
        _clientView.StartCoroutine(GoToBase());
    }

    private IEnumerator GoToBase() {
        _tapped = false;
        _expired = false;
        _served = false;
        _destinationReached = false;
        _clientLegs.GoTo(_clientController.HomePosition);
        while(true) {
            if(_destinationReached) {
                _clientView.StartCoroutine(Idle());
                yield break;
            } else if(_tapped) {
                _clientLegs.Stop();
                _clientView.StartCoroutine(GoToWait());
                yield break;
            }
            yield return null;
        }
    }

    private void GoToQueue()
    {
        _queue.AddClientInQueue(_clientView); 
        _clientLegs.GoTo(_queue.PositionMath(_clientView));
        _clientView.StartCoroutine(GoToWait());
    }

    public void OnTapped(){
        _tapped = true;
    }
    public void OnServed(){
        _served = true;
    }
    public void OnExpired(){
        _expired = true;
    }
    public void OnDestinationReached(){
        _destinationReached = true;
    }
    public void OnEmotePlayed(){
        _emotePlayed = true;
    }

    public void GoTo(Vector3 position)
    {
        _clientLegs.GoTo(position);
    }

    public void Stop()
    {
        _clientLegs.Stop();
    }
    }
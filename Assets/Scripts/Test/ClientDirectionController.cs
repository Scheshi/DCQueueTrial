using System.Collections;
using UnityEngine;


    public class ClientDirectionController : IBrain
    {
        //Потому что строки - раковая опухоль шарпов
        private const string HAPPY_EMOTY = "Happy";
        
        private Mutex _mutex;
        private QueueController _queue;
        private Client _client;
        private Legs _clientLegs;
        private Animator _animator;
        
        private bool _tapped;
        private bool _served;
        private bool _expired;
        private bool _destinationReached;
        private bool _emotePlayed;
        public ClientDirectionController(Mutex mutex, QueueController queue, Client client, Legs legs, Animator animator)
        {
            _mutex = mutex;
            _queue = queue;
            _client = client;
            _clientLegs = legs;
            _animator = animator;
            _client.StartCoroutine(Idle());
        }
        

        IEnumerator Idle()
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

    IEnumerator GoToWait()
    {
        _tapped = false;
        _expired = false;
        _served = false;
        _destinationReached = false;
        while (true)
        {
            IEnumerator mutexLock = _mutex.Lock(_client);
            if (_queue.IndexOfClient(_client) == 0 && !mutexLock.MoveNext())
            {
                _queue.RemoveClientInQueue(_client);
                _client.StartCoroutine(GoToServer());
                yield break;
            } else if(_tapped) {
                _queue.RemoveClientInQueue(_client);
                _client.StartCoroutine(GoToBase());
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
                
                _client.StartCoroutine(WaitForServed());
                yield break;
            } else if(_tapped) {
                _client.StartCoroutine(GoToBase());
                _mutex.Unlock(_client);
                yield break;
            }
            yield return null;
        }
    }
    
    IEnumerator WaitForServed() {
        _tapped = false;
        _expired = false;
        _served = false;
        _destinationReached = false;
        _client.OrderStuff();
        Server server = GameObject.FindObjectOfType<Server>();
        server.currentClient = _client;
        while(true) {
            if(_served) {
                _client.StartCoroutine(PlayEmote());
                server.currentClient = null;
                yield break;
            } else if(_expired) {
                _client.StartCoroutine(GoToBase());
                _mutex.Unlock(_client);
                server.currentClient = null;
                yield break;
            } else if(_tapped) {
                _client.ExpireNow();
            }
            yield return null;
        }
    }

    IEnumerator PlayEmote() {
        _emotePlayed = false;
        _animator.SetTrigger(HAPPY_EMOTY);
        while(!_emotePlayed) {
            yield return null;
        }
        _mutex.Unlock(_client);
        _client.StartCoroutine(GoToBase());
    }

    IEnumerator GoToBase() {
        _tapped = false;
        _expired = false;
        _served = false;
        _destinationReached = false;
        _clientLegs.GoTo(_client.HomePosition);
        while(true) {
            if(_destinationReached) {
                _client.StartCoroutine(Idle());
                yield break;
            } else if(_tapped) {
                _clientLegs.Stop();
                _client.StartCoroutine(GoToWait());
                yield break;
            }
            yield return null;
        }
    }

    private void GoToQueue()
    {
        _queue.AddClientInQueue(_client); 
        _clientLegs.GoTo(_queue.PositionMath(_client));
        _client.StartCoroutine(GoToWait());
    }

    public void OnTapped(){
        Debug.Log("Нажат!");
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
}
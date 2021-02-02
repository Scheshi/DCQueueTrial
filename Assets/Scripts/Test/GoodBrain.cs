using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodBrain : MonoBehaviour, IBrain
{
    private bool _tapped;
    private bool _served;
    private bool _expired;
    private bool _destinationReached;
    private bool _emotePlayed;
    private Mutex _mutex;
    private QueueController _queue;
    private Client _client;
    
    
    void Start()
    {
        _mutex = FindObjectOfType<Mutex>();
        _queue = FindObjectOfType<QueueController>();
        _client = GetComponent<Client>();
        StartCoroutine(Idle());
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
            IEnumerator mutexLock = _mutex.Lock(this);
            if (_queue.IndexOfClient(_client) == 0 && !mutexLock.MoveNext())
            {
                _queue.RemoveClientInQueue(_client);
                StartCoroutine(GoToServer());
                yield break;
            } else if(_tapped) {
                _queue.RemoveClientInQueue(_client);
                StartCoroutine(GoToBase());
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
        GetComponent<Legs>().GoTo(_mutex.transform.position);
        while(true) {
            if(_destinationReached) {
                
                StartCoroutine(WaitForServed());
                yield break;
            } else if(_tapped) {
                StartCoroutine(GoToBase());
                _mutex.Unlock(this);
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
        GetComponent<Client>().OrderStuff();
        Server server = GameObject.FindObjectOfType<Server>();
        server.currentClient = GetComponent<Client>();
        while(true) {
            if(_served) {
                StartCoroutine(PlayEmote());
                server.currentClient = null;
                yield break;
            } else if(_expired) {
                StartCoroutine(GoToBase());
                _mutex.Unlock(this);
                server.currentClient = null;
                yield break;
            } else if(_tapped) {
                GetComponent<Client>().ExpireNow();
            }
            yield return null;
        }
    }

    IEnumerator PlayEmote() {
        _emotePlayed = false;
        GetComponent<Animator>().SetTrigger("Happy");
        while(!_emotePlayed) {
            yield return null;
        }
        _mutex.Unlock(this);
        StartCoroutine(GoToBase());
    }

    IEnumerator GoToBase() {
        _tapped = false;
        _expired = false;
        _served = false;
        _destinationReached = false;
        GetComponent<Legs>().GoTo(GetComponent<Client>().home.transform.position);
        while(true) {
            if(_destinationReached) {
                StartCoroutine(Idle());
                yield break;
            } else if(_tapped) {
                GetComponent<Legs>().Stop();
                StartCoroutine(GoToWait());
                yield break;
            }
            yield return null;
        }
    }

    private void GoToQueue()
    {
        _queue.AddClientInQueue(_client); 
        GetComponent<Legs>().GoTo(_queue.PositionMath(_client));
            //var toPosition = _queue.PositionMath(_client);
            //_client.GetComponent<Legs>().GoTo(toPosition);
        StartCoroutine(GoToWait());
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
}

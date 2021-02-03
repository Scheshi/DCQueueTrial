using System;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoBehaviour
{
    private readonly List<IClientView> _clientQueue = new List<IClientView>();
    private Transform[] _queuePosition;

    private void Start()
    {
        //_mutex = FindObjectOfType<Mutex>();
        var markers = transform.GetComponentsInChildren<QueueMarker>();
        _queuePosition = new Transform[markers.Length];
        for (var i = 0; i < markers.Length; i++)
        {
            _queuePosition[i] = markers[i].transform;
        }
        
        var serverPosition = FindObjectOfType<Server>().transform.position;

        var pc = new PositionComparer(serverPosition);

        Array.Sort(_queuePosition, pc);
        
    }

    public void AddClientInQueue(IClientView clientView)
    {
        _clientQueue.Add(clientView);
    }

    public void RemoveClientInQueue(IClientView clientView)
    {
        _clientQueue.Remove(clientView);
        foreach (var item in _clientQueue)
        {
            if(item.Brain is ILegs)
                (item.Brain as ILegs).GoTo(PositionMath(item));
        }
    }

    public int IndexOfClient(IClientView clientView)
    {
        return _clientQueue.IndexOf(clientView);
    }
    
    public Vector3 PositionMath(IClientView clientView)
    {
        if (_clientQueue.Contains(clientView))
        {
            var optionClient = _clientQueue.IndexOf(clientView);
            return _queuePosition[optionClient].position;
        }
        else throw new NullReferenceException("Клиента нет в очереди");
    }
}

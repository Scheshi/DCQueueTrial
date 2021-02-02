using System;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoBehaviour
{
    private readonly List<Client> _clientQueue = new List<Client>();
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

    public void AddClientInQueue(Client client)
    {
        _clientQueue.Add(client);
    }

    public void RemoveClientInQueue(Client client)
    {
        _clientQueue.Remove(client);
        foreach (var item in _clientQueue)
        {
            item.GetComponent<Legs>().GoTo(PositionMath(item));
        }
    }

    public int IndexOfClient(Client client)
    {
        return _clientQueue.IndexOf(client);
    }
    
    public Vector3 PositionMath(Client client)
    {
        if (_clientQueue.Contains(client))
        {
            var optionClient = _clientQueue.IndexOf(client);
            return _queuePosition[optionClient].position;
        }
        else throw new NullReferenceException("Клиента нет в очереди");
    }
}

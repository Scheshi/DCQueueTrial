using System;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoBehaviour
{
    public List<Client> ClientQueue = new List<Client>();
    public Transform[] _queuePosition;

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
        ClientQueue.Add(client);
    }

    public void RemoveClientInQueue(Client client)
    {
        ClientQueue.Remove(client);
        foreach (var item in ClientQueue)
        {
            item.GetComponent<Legs>().GoTo(PositionMath(item));
        }
    }

    public int IndexOfClient(Client client)
    {
        return ClientQueue.IndexOf(client);
    }
    
    public Vector3 PositionMath(Client client)
    {
        if (ClientQueue.Contains(client))
        {
            var optionClient = ClientQueue.IndexOf(client);
            return _queuePosition[optionClient].position;
        }
        else throw new NullReferenceException("Клиента нет в очереди");
    }
}

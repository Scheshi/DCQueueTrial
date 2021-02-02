using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoBehaviour
{
    [SerializeField]private readonly List<Client> ClientQueue = new List<Client>();
    private Transform[] _queuePosition;
    private Mutex _mutex;

    private void Start()
    {
        _mutex = FindObjectOfType<Mutex>();
        _queuePosition = GetComponentsInChildren<Transform>();
        var serverPosition = FindObjectOfType<Server>().transform.position;

        var pc = new PositionComparer(serverPosition);

        Array.Sort(_queuePosition, pc);
    }

    public void AddClientInQueue(Client client)
    {
        ClientQueue.Add(client);
        client.GetComponent<Legs>().GoTo(PositionMath(client));
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

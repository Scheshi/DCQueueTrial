using System;
using System.Collections.Generic;
using UnityEngine;

public class QueueController
{
    private readonly List<IClientView> _clientQueue = new List<IClientView>();
    private Transform[] _queuePoints;

    public QueueController(Vector3 mutexPosition, Transform[] queuePoints)
    {
        _queuePoints = queuePoints;
        Start(mutexPosition);
    }
    
    private void Start(Vector3 mutexPosition)
    {
        var pc = new PositionComparer(mutexPosition);
        Array.Sort(_queuePoints, pc);
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
            return _queuePoints[optionClient].position;
        }
        else throw new NullReferenceException("Клиента нет в очереди");
    }
}

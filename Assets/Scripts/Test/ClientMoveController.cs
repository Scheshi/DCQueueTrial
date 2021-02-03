using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ClientMoveController : ILegs
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private Coroutine _walk = null;
    private IClientView _clientView;
    private MonoBehaviour _monoParser;
    private float _speed;

    public ClientMoveController(IClientView clientView, NavMeshAgent agent, Animator animator, float speed)
    {
        if (clientView is MonoBehaviour)
        {
            _clientView = clientView;
            _monoParser = _clientView as MonoBehaviour;
        }
        else throw new ArgumentException("IClientView должен быть абстракцией MonoBehaviour");

        _speed = speed;
        _animator = animator;
        _agent = agent;
    }

    public void GoTo(Vector3 position) {
        Stop();
        _agent.destination = position;
        _agent.updateRotation = false;
        _agent.speed = _speed;
        _walk = _monoParser.StartCoroutine(WalkRoutine());
    }

    private IEnumerator WalkRoutine()
    {
        while(_agent.pathPending) yield return null;
        while(_agent.remainingDistance > _agent.stoppingDistance) 
        {
            _animator.SetFloat("Speed", _agent.speed);
            yield return null;
        }
        _animator.SetFloat("Speed", 0f);
        _clientView.Brain.OnDestinationReached();
    }

    public void Stop()
    {
        _animator.SetFloat("Speed", 0f);
        if(_walk != null) _monoParser.StopCoroutine(_walk);
        _walk = null;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Legs : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Coroutine walk = null;
    private Client _client;
    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        _client = GetComponent<Client>();
    }
    public void GoTo(Vector3 position) {
        Stop();
        agent.destination = position;
        agent.updateRotation = false;
        agent.speed = _client?.Speed ?? agent.speed;
        walk = StartCoroutine(WalkRoutine());
    }

    private IEnumerator WalkRoutine()
    {
        while(agent.pathPending) yield return null;
        while(agent.remainingDistance > agent.stoppingDistance) 
        {
            animator.SetFloat("Speed", agent.speed);
            yield return null;
        }
        animator.SetFloat("Speed", 0f);
        GetComponent<IBrain>().OnDestinationReached();
    }

    public void Stop()
    {
        animator.SetFloat("Speed", 0f);
        if(walk != null) StopCoroutine(walk);
        walk = null;
    }
}

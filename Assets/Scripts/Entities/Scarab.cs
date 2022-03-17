using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Scarab : Monster
{
    private NavMeshAgent agent;
    private void Awake()
    {
        Init();
        Spawn(); 
        agent = gameObject.GetComponent<NavMeshAgent>();
    }
    private void Update() => _stateMachine.Tick();

    public override void Idle()
    {
        agent.SetDestination(player.transform.position);
        if (agent.hasPath)
            agent.acceleration = (agent.remainingDistance < 1) ? 60 : 2;
    }
}

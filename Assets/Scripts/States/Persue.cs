using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class Persue : MonoBehaviour, IState
{
    private Monster monster;
    private GameObject target;
    
    public void Tick()
    {
        ChaseTarget();
    }

    public Persue(Monster monster, GameObject target)
    {
        this.monster = monster;
        this.target = target;
    }

    void ChaseTarget()
    {
        if (monster.mobCard.movementType == Monster.MovementType.Walking)
        {
            monster.agent.SetDestination(target.transform.position);
        }
        monster.transform.position = Vector3.MoveTowards(monster.transform.position, target.transform.position, Time.deltaTime * monster.stats.Agility);
    }

    public void OnEnter()
    {
        Debug.Log("persue");
    }

    public void OnExit()
    {
        
    }
}

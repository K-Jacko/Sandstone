using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Attack : MonoBehaviour, IState
{
    private readonly Monster monster;
    private GameObject target;
    private bool isAttacking;
    private Stopwatch timer;
    
    public void Tick()
    {
        Hit();
        monster.gameObject.transform.LookAt(target.transform);
    }

    public Attack(Monster monster, GameObject target)
    {
        this.monster = monster;
        this.target = target;
    }

    void Hit()
    {
        if (monster.ManaPool >= monster.mobCard.ability.manaCost & timer.Elapsed.Seconds > 2f)
        {
            monster.Cast(monster.mobCard.ability, target.GetComponent<Entity>());
            timer.Restart();
            
        }
        else
        {
            
            if (timer.Elapsed.Seconds > 2f)
            {
                monster.Refactor();
            }
            
        }
    }

    public void OnEnter()
    {
        timer = Stopwatch.StartNew();
    }

    public void OnExit()
    {
       
    }
}

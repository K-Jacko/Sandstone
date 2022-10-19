using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Firefly : Monster
{
    public override void InitAttackStates()
    {
        movementStateMachine = new StateMachine();
        combatStateMachine = new StateMachine();
        
        var persue = new Persue(this,target);
        var idle = new Idle();
        var attack = new Attack(this, target);
        
        movementStateMachine.AddAnyTransition(persue,PlayerNotInRange());
        movementStateMachine.AddAnyTransition(idle,PlayerInRange());
        combatStateMachine.AddAnyTransition(idle,PlayerNotInRange());
        combatStateMachine.AddAnyTransition(attack,PlayerInRange());
        
        movementStateMachine.SetState(persue);
        combatStateMachine.SetState(idle);
        
        Func<bool> PlayerNotInRange() => () =>
        {
            var distance = Vector3.Distance(transform.position, target.gameObject.transform.position);
            return (distance >= combatRadius);
        };
        //
        Func<bool> PlayerInRange() => () =>
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);
            return (distance < combatRadius);
        };
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f,1f,0f,0.1f);
        var position = gameObject.transform.position;
        Gizmos.DrawSphere(position, combatRadius);

    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TestEntity : Monster
{
    private StateMachine _stateMachine;
    public int tempBool = 3;
    public Material TEMaterial;
    public bool leaving;

    // Start is called before the first frame update
    void Awake()
    {
        // tempBool = 3;
        // GameObject o;
        // TEMaterial = (o = gameObject).GetComponent<MeshRenderer>().sharedMaterial;
        // _stateMachine = new StateMachine();
        //
        //
        // // var enterColor = new Attack(this,TEMaterial,o);
        // // var exitColor = new Cooldown(this,TEMaterial,o);
        // // var idleColor = new Idle(this,TEMaterial,o);
        //
        // At(idleColor, enterColor, CloseToTarget());
        // _stateMachine.AddAnyTransition(enterColor, () => tempBool == 0);
        // At(enterColor, exitColor, AwayFromTarget());
        // At(exitColor, idleColor, ExitedAndAway());
        //
        // _stateMachine.SetState(idleColor);
        //
        // void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
        //
        // Func<bool> CloseToTarget() => () => tempBool == 0;
        // Func<bool> AwayFromTarget() => () => tempBool == 1;
        // Func<bool> ExitedAndAway() => () => tempBool == 3;


    }
    
    private void Update() => _stateMachine.Tick();

    public void OnTriggerEnter(Collider collider)
    {
        tempBool = 0;
    }

    public void OnTriggerExit(Collider collider)
    {
        tempBool = 1;
        if(!leaving)
            StartCoroutine(Leaving());
    }
    
    private IEnumerator Leaving()
    {
        leaving = true;
        yield return new WaitForSeconds(3);
        tempBool = 3;
        leaving = false;
    }
}

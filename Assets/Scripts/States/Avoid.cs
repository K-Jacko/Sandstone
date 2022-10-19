using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Avoid : MonoBehaviour, IState
{
    private Monster monster;
    private GameObject target;
    private float rotationSpeed = 10;
    public void Tick()
    {
        MoveMod();
    }

    public Avoid(Monster monster, GameObject target)
    {
        this.monster = monster;
        this.target = target;
    }

    public void MoveMod()
    {
        
        float x = -Mathf.Cos(Time.timeSinceLevelLoad ) * monster.combatRadius / 2; //* monster.combatRadius/2;
        float z = Mathf.Sin(Time.timeSinceLevelLoad) * monster.combatRadius / 2 ;
        var position = monster.gameObject.transform.position;
        Vector3 pos = new Vector3(x, 0, z);
        var position1 = target.transform.position;
        monster.agent.SetDestination(position = pos + position1);

        monster.gameObject.transform.LookAt(target.transform);
        
    }

    public void OnEnter()
    {
        Debug.Log("avoid");
        monster.agent.speed = monster.stats.Agility;
    }

    public void OnExit()
    {
        
    }
}

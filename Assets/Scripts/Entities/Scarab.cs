using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Scarab : Monster
{
    public float rotationSpeed = 80.0f;
    public float radius = 2.0f;
    
    private NavMeshAgent agent;
    private float timer = 0;
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
            agent.acceleration = (agent.remainingDistance < 1) ? speed : 2;
        else
        {
            // RaycastHit hit;
            // var layerMask = 1 << 6;
            // if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, layerMask))
            // {
            //     transform.position = hit.point;
            // }
        }
    }

    public override void Attack()
    {
        var position = player.transform.position;
        //transform.RotateAround (position, Vector3.up, rotationSpeed * Time.deltaTime);
        Vector3  desiredPosition;

        //Rotate Around
        //desiredPosition = ((transform.position - position).normalized * radius + position) * Mathf.Sin(1); 
        
        //transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * speed);
        timer += Time.deltaTime * speed;
        transform.LookAt(player.transform);
        float x = -Mathf.Cos(timer) * rotationSpeed;
        float z = Mathf.Sin(timer) * rotationSpeed;
        Vector3 pos = new Vector3(x, 0, z);
        agent.SetDestination(pos + position);
    }
}

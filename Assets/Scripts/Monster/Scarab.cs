using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Scarab : Monster
{
    public Vector3 followtarget;
    public Swarm swarm;
    private void Awake()
    {
        swarm = gameObject.GetComponentInParent<Swarm>();
        followtarget = swarm.transform.position;
    }

    void Update()
    {
        float jitter = swarm.conf.wanderJitter * Time.fixedTime;
        followtarget += new Vector3(RandomBinomial() * jitter, RandomBinomial() * jitter, RandomBinomial() * jitter);
        transform.position = Vector3.MoveTowards(gameObject.transform.position,
            followtarget, conf.attackSpeed);
    }
    
    float RandomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }
}

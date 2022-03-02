using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Vector3 spawnPoint;
    public float spawnRadius;
    public float combatRadius;
    public GameObject player;
    public GameObject monster;
    public float speed;

    public virtual void Attack()
    {
        float step = speed * Time.deltaTime;
        gameObject.transform.position =
            Vector3.MoveTowards(monster.transform.position, player.transform.position, step);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void Return()
    {
        float step = (speed * Time.deltaTime) * 10f;
        gameObject.transform.position =
            Vector3.MoveTowards(monster.transform.position, spawnPoint, step);
    }
}

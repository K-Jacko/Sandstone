using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    public Transform monsterPrefab;
    public Transform playerPrefab;
    public int numberOfFriendlies;
    public int numberOfMonsters;
    public float lookInterval = 2f;
    public List<Monster> monsters;
    public List<Friendly> friendlies;
    public float bounds;
    public float spawnRadius;
    public float combatRadius;
    public SphereCollider spawnerCollider;
    
    private bool isObservingFriendly = false;
    private Material spawnerMaterial;
    private MonsterConfig conf;

    public bool alignmentEnabled;
    public bool separationEnabled;
    public bool cohesionEnabled;
    public bool seekEnabled;
    public bool wanderEnabled;
    // Start is called before the first frame update
    void Start()
    {
        conf = gameObject.GetComponent<MonsterConfig>();
        spawnerCollider = gameObject.GetComponent<SphereCollider>();
        spawnerCollider.radius = combatRadius;
        spawnerMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
        spawnerMaterial.color = Color.gray;
        
        monsters = new List<Monster>();
        friendlies = new List<Friendly>();
    }

    void FixedUpdate()
    {
        // Note: this doesn't conflict with adding boids at runtime because everything executes in one frame
        foreach (Swarm monster in monsters)
        {
            Swarm boid = monster.GetComponent<Swarm>();
            Rigidbody body = boid.GetComponent<Rigidbody>();

            if (alignmentEnabled)
            {
                body.AddForce(boid.Align() * conf.alignmentRadius);
            }

            if (separationEnabled)
            {
                body.AddForce(boid.Separate() * conf.seprationRadius);
            }

            if (cohesionEnabled)
            {
                body.AddForce(boid.Cohere() * conf.cohesionRadius);
            }

            if (seekEnabled)
            {
                body.AddForce(boid.Seek(friendlies[0].gameObject.transform.position) * conf.attackSpeed / 100);
            }

            if (wanderEnabled)
            {
                body.AddForce(boid.Wander() * conf.wanderRadius);
            }
        }
    }

    void Spawn()
    {
        for (int i = 0; i < numberOfMonsters; i++)
        {
            var go = Instantiate(monsterPrefab, gameObject.transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius)),Quaternion.identity);
            go.transform.SetParent(gameObject.transform);
            monsters.Add(go.GetComponent<Monster>());
        }
    }

    public IEnumerator LookForPlayer(Friendly friendly)
    {
        spawnerMaterial.color = Color.yellow;
        yield return new WaitForSeconds(lookInterval);
        if (friendlies.Count <= 0)
        {
            ShutDown();
        }
        else
        {
            yield return null;
        }

    }

    void Active(Collider friendly = null)
    {
        Spawn();
        friendlies.Clear();
        friendlies.AddRange(FindObjectsOfType<Friendly>());
        
        isObservingFriendly = true;
        spawnerMaterial.color = Color.red;
    }

    void ShutDown()
    {
        isObservingFriendly = false;
        spawnerMaterial.color = Color.gray;

        foreach (var monster in monsters)
        {
            monster.Kill(monster.gameObject);
        }
        monsters.Clear();
        friendlies.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Friendly>())
        {
            Active(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Friendly>())
        {
            if(other.GetComponent<Friendly>() == friendlies[0])
                friendlies.Clear();
            StartCoroutine(LookForPlayer(other.gameObject.GetComponent<Friendly>()));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f,1f,1f,0.5f);
        var position = gameObject.transform.position;
        Gizmos.DrawSphere(position, spawnRadius);
        Gizmos.color = new Color(0.5f,1f,0.5f,0.2f);
        Gizmos.DrawSphere(position, combatRadius);
    }
}

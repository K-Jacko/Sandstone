using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Swarm : Monster
{

    // Note: maxSpeed should always be < maxForce
    public float maxSpeed = 10;
    public float maxForce = 20;
    public GameObject monster;
    public MonsterConfig conf;

    // For wandering only
    private int timeLookahead = 4; 
    private float wanderingRadius = 60;
    private float numberOfMonsters;

    private Rigidbody body;

    private MonsterSpawner monsterSpawner;

    private Vector3 fPos;

    private void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        monsterSpawner = gameObject.GetComponentInParent<MonsterSpawner>();
        conf = monsterSpawner.GetComponentInParent<MonsterConfig>();
        numberOfMonsters = monsterSpawner.numberOfMonsters * 0.5f;
        // for (int i = 0; i < numberOfMonsters; i++)
        // {
        //     var go = Instantiate(monster, gameObject.transform.position + new Vector3(Random.Range(-monsterSpawner.spawnRadius, monsterSpawner.spawnRadius), 2, Random.Range(-monsterSpawner.spawnRadius, monsterSpawner.spawnRadius)),Quaternion.identity);
        //     go.transform.SetParent(gameObject.transform);
        // }
    }

    private void FixedUpdate()
    {
        fPos = monsterSpawner.friendlies[0].transform.position;
        if (Vector3.Distance(fPos, transform.position) > 5f)
        {
            if (body.velocity.magnitude > maxForce)
            {
                body.velocity = new Vector3(maxForce, maxForce, maxForce);
            }
        }
        
    }

    public Vector3 Separate()
    {
        Vector3 totalSeparation = Vector3.zero;
        int numNeighbors = 0;

        // Loop through all boids in the world
        foreach(Swarm monster in monsterSpawner.monsters)
        {
            Monster neighbor = monster.GetComponent<Monster>();

            Vector3 separationVector = transform.position - neighbor.transform.position;
            float distance = separationVector.magnitude;

            // If it's a neighbor within our vicinity
            if(distance > 0 && distance < monster.conf.seprationRadius)
            {
                separationVector.Normalize();

                // The closer a neighbor (smaller the distance), the more we should flee
                separationVector /= distance;

                totalSeparation += separationVector;
                numNeighbors++;
            }
        }

        // That is, if this boid actually has neighbors to worry about
        if(numNeighbors > 0)
        {
            // Compute its average separation vector
            Vector3 averageSeparation = totalSeparation / numNeighbors;
            averageSeparation.Normalize();
            averageSeparation *= maxSpeed;

            // Compute the separation force we need to apply
            Vector3 separationForce = averageSeparation - body.velocity;

            // Cap that separation force
            if (separationForce.magnitude > maxForce)
            {
                separationForce.Normalize();
                separationForce *= maxForce;
            }
            
            return separationForce;
        }

        return Vector3.zero;
    }


    /* Called each frame by the group manager. Aligns this boid with all other boids
     * in its immediate neighborhood, effectively making it travel in the same direction.
     */
    public Vector3 Align()
    {
        Vector3 totalHeading = Vector3.zero;
        int numNeighbors = 0;

        // Loop through all boids in the world
        foreach (Swarm swarm in monsterSpawner.monsters)
        {
            Monster monster = swarm.GetComponent<Monster>();

            Vector3 separationVector = transform.position - monster.transform.position;
            float distance = separationVector.magnitude;

            // If it's a neighbor within our vicinity
            if (distance > 0 && distance < monster.conf.alignmentRadius)
            {
                numNeighbors++;
                totalHeading += swarm.body.velocity.normalized;
            }
        }

        // That is, if this boid actually has neighbors to worry about
        if (numNeighbors > 0)
        {
            // Average direction we need to head in
            Vector3 averageHeading = (totalHeading / numNeighbors);
            averageHeading.Normalize();

            // Compute the steering force we need to apply
            Vector3 alignmentForce = averageHeading * maxSpeed;

            // Cap that steering force
            if (alignmentForce.magnitude > maxForce)
            {
                alignmentForce.Normalize();
                alignmentForce *= maxForce;
            }

            return alignmentForce;
        }

        return Vector3.zero;
    }
    

    /* Returns a vector that can be used to direct a boid towards the target position.
     */ 
    public Vector3 Seek(Vector3 target)
    {
        // Force to be applied to the boid
        Vector3 steerForce = GetDesiredVelocity(target) - body.velocity;

        // Cap the force that can be applied
        if (steerForce.magnitude > maxForce)
        {
            steerForce = steerForce.normalized * maxForce;
        }

        return steerForce;
    }


    /* Called each frame by the group manager. Ensures this boid remains close to neighboring boids.
    */
    public Vector3 Cohere()
    {
        Vector3 totalPositions = Vector3.zero;
        int numNeighbors = 0;

        // Loop through all boids in the world
        foreach (Swarm monster in monsterSpawner.monsters)
        {
            Swarm boid = monster.GetComponent<Swarm>();

            Vector3 separationVector = transform.position - boid.transform.position;
            float distance = separationVector.magnitude;

            // If it's a neighbor within our vicinity, add its position to cumulative
            if (distance > 0 && distance < monster.conf.cohesionRadius)
            {
                print(distance);
                numNeighbors++;
                totalPositions += boid.body.velocity.normalized;
            }
        }

        // If there are neighbors
        if(numNeighbors > 0)
        {
            Vector3 averagePosition = (totalPositions / numNeighbors) + fPos;

            return Seek(averagePosition);
        }

        return Vector3.zero;
    }


    /* Allows a boid to wander aimlessly, though with some sense of direction.
     */ 
    public Vector3 Wander()
    {
        // Select random point on circle of radius "radius" around the future position
        Vector3 target = GeneratePointOnCircle(GetFuturePosition());

        // Compute desired velocity as one pointing there
        Vector3 desiredVelocity = GetDesiredVelocity(target);

        // Get the steering force vector
        Vector3 steerForce = desiredVelocity - body.velocity;

        // Cap the force that can be applied
        if (steerForce.magnitude > maxForce)
        {
            steerForce = steerForce.normalized * maxForce;
        }

        return steerForce;
    }


    /* Returns a random point on a circle positioned at the given center and radius.
     */
    Vector3 GeneratePointOnCircle(Vector3 center)
    {
        Vector3 point = center;

        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        point.x += wanderingRadius * Mathf.Cos(angle);
        point.z += wanderingRadius * Mathf.Sin(angle);

        return point;
    }


    /* Computes and returns the future, predicted position of this object, assuming
     * it continues traveling in its current direction at its current speed.
     */
    Vector3 GetFuturePosition()
    {
        return transform.position + body.velocity * timeLookahead;
    }



    /* The desired velocity is simply the unit vector in the direction of the target
     * scaled by the speed of the object.
     */
    Vector3 GetDesiredVelocity(Vector3 target)
    {
        return (target - transform.position).normalized * maxSpeed;
    }
}

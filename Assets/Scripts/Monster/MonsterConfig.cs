using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterConfig : MonoBehaviour
{
   public float maxFOV = 180;
   public float maxAcceleration;
   public float maxVelocity;
   
   public float wanderJitter;
   public float wanderRadius;
   public float wanderDistance;
   public float wanderPriority;
   
   public float cohesionRadius;
   public float cohesionPriority;
   
   public float alignmentRadius;
   public float alignmentPriority;

   public float seprationRadius;
   public float seperationPriority;
   
   public float avoidanceRadius;
   public float avoidancePriority;

   public float attackInterval;
   public float attackSpeed;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class Entity : MonoBehaviour
{
   public int Stamina;
   public int Focus;
   public int Agility;
   public int Power;
   public Ability ability;

   public bool isCasting;
   public InputActionProperty castRefference;

   public int ManaPool;
   
   private void Awake()
   {
      
   }

   private void Update()
   {
     
   }

   public virtual void Cast(Ability spell, Entity target)
   {
      switch (spell.projectileType)
      {
         case(Ability.ProjectileType.Laser) :
            CastLaserSpell(spell);
            break;
         case Ability.ProjectileType.Physics :
            CastPhysicsSpell(spell, target);
            break;
         case Ability.ProjectileType.Pulse :
            CastPulseSpell(spell);
            break;
      }
   }

   public virtual void CastLaserSpell(Ability spell)
   {
      
   }
   public virtual void CastPhysicsSpell(Ability spell, Entity target)
   {
      //Expend mana
      //Make primative from power and suffix. 
      //project primative using physics and power
      //Minus health from entity
      ManaPool -= spell.manaCost;
      var go = GameObject.CreatePrimitive(spell.suffix);
      go.AddComponent<Projectile>();
      go.transform.position = transform.position;
      go.transform.localScale = new Vector3(spell.power + Power, spell.power + Power , spell.power + Power ) / 100;
      go.layer = 7;
      var rigidBody = go.AddComponent<Rigidbody>();
      rigidBody.isKinematic = false;
      rigidBody.useGravity = false;
      rigidBody.velocity = (target.transform.position - transform.position).normalized * Power;
      go.transform.parent = transform;
      if (ManaPool > 0)
      {
         isCasting = false;
      }
      else if(ManaPool == 0)
      {
         StartCoroutine(Refactor());
      }
   }
   public virtual void CastPulseSpell(Ability spell)
   {
      //Expend Mana
      //Enhance Equiptable
      //Minus health from entity
      //
   }

   public virtual bool CheckManaPool()
   {
      return false;
   }

   public virtual IEnumerator Refactor()
   {
      Debug.Log("Reloading");
      yield return new WaitForSeconds(2);
      isCasting = false;
      ManaPool = Focus;

   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : XRGrabInteractable
{
    public enum WeaponType {Ranged, Melee, Other}
    public WeaponType weaponType;
    
    public Transform firePoint;
    public float Force;
    public GameObject projectilePrefab;
    public ProjectilePool projectilePool;
    public int Ammo;
    public float reloadTime = 1.5f;
    public int ammoScale = 1;


    private bool isReloading = false;
    private int firedCount = 0;

    // Start is called before the first frame update
    void Start()
    {
       switch(weaponType)
       {
           case Weapon.WeaponType.Ranged :
           UpdateFiredCount(0);
           break;

           case WeaponType.Melee :
           
           break;

           case WeaponType.Other :

           break;
       }
      

    }
    private void Update()
    {
        if(isReloading)
            return;
    }
    
    public void Fire()
    {
        if (firedCount >= Ammo)
            return;

        Projectile targetProjectile = projectilePool.Projectiles[firedCount];
        targetProjectile.Launch(this);

        UpdateFiredCount(firedCount + 1);
       
    }

    public virtual IEnumerator Reload(InputAction.CallbackContext context)
    {
        if (firedCount == 0)
        yield break;

        isReloading = true;
        projectilePool.SetAllProjectiles();

        yield return new WaitForSeconds(reloadTime);

        UpdateFiredCount(0);
        isReloading = false;
    }

    private void UpdateFiredCount(int newValue)
    {
        firedCount = newValue;
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : Weapon
{

    public InputActionReference reload;
    
    // Start is called before the first frame update
    void Start()
    {
        weaponType = WeaponType.Ranged;
        projectilePool = new ProjectilePool(projectilePrefab, Ammo, gameObject);
        reload.action.performed += ReloadVoid;
    }

    private void ReloadVoid(InputAction.CallbackContext context)
    {
        StartCoroutine(Reload(context));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

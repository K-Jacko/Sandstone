using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Staff : XRTwoHandGrabInteractable
{
    public InputActionReference reload = null;

    protected override void Awake()
    {
        base.Awake();
        weaponType = WeaponType.Ranged;
        projectilePool = new ProjectilePool(projectilePrefab, Ammo, gameObject);
        reload.action.performed += ReloadVoid;
    }

    private void ReloadVoid(InputAction.CallbackContext context)
    {
        StartCoroutine(Reload(context));
    }

    public override IEnumerator Reload(InputAction.CallbackContext context)
    {
        StartCoroutine(base.Reload(context));
        yield return new WaitForSeconds(reloadTime);
    }

}

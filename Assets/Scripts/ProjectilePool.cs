using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : Pool
{
    public List<Projectile> Projectiles = new List<Projectile>();
    public ProjectilePool(GameObject prefab, int count, GameObject parent)
    {
        Projectiles = Create<Projectile>(prefab, count, parent);
    }

    public void SetAllProjectiles()
    {
        foreach(Projectile projectile in Projectiles)
            projectile.SetInnactive();
    }
}
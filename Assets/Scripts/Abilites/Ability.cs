using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ability,", menuName = "Ability")]
public class Ability : ScriptableObject
{
    public enum ProjectileType{Physics, Laser, Pulse}
    public ProjectileType projectileType;
    public int manaCost;
    public int power;
    public PrimitiveType suffix;
}
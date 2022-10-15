using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats 
{
    public int Stamina;
    public int Power;
    public int Agility;
    public int Focus;
    
    public GemElement GemElement; // TODO: refactor this. You need to think about what needs to change in mutations

    public Stats(int stamina, int power, int agility, int focus, GemElement element)
    {
        this.Stamina = stamina;
        this.Power = power;
        this.Agility = agility;
        this.Focus = focus;
        this.GemElement = element;
    }
}

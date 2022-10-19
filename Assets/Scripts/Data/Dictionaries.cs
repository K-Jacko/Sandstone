using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dictionaries 
{
    public static Dictionary<String, Scene> MapRoster = new Dictionary<string, Scene>();

    public static Dictionary<int, string> MobRoster = new Dictionary<int, string>();

    public Dictionary<int, string> MobCardRoster
    {
        get
        {
            MobRoster.Add(0,"FireFly");
            MobRoster.Add(1,"Scarab");
            return MobRoster;
        }
    }

    public static String[] GetValidMobCards(int[] index)
    {
        MobRoster.Add(0,"FireFly");
        MobRoster.Add(1,"Scarab");
        List<String> mobCards = new List<String>();
        foreach (var hex in index)
        {
            mobCards.Add(Resources.Load<MobCard>("MobCards").name);
        }

        return mobCards.ToArray();
    }
}


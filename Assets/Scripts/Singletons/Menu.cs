using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public bool test;
    private void Update()
    {
        if (test)
        {
            SceneDirector.Instance.LoadLoading("Test Stage", null);
            test = false;
        }
    }
}

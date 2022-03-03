using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inferno : GemElement
{
    private int _element = 2;

    public void Mutate()
    {
        //This needs to happen in the init of a entity 
        throw new System.NotImplementedException();
    }
    
    public int GetElement()
    {
        //this ovsly happens before Mutate
        return _element;
    }
    
}

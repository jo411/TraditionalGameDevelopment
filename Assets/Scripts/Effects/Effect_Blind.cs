using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Blind : Effect
{
    double savedAccuracy = 0.0;

    public bool update(bool isPlayerTurn, Entity affected)
    {
        throw new NotImplementedException();
    }
    public override string ToString()
    {
        return "Blind";
    }
        
    void Effect.apply(Entity affected)
    {
        throw new NotImplementedException();
    }
}

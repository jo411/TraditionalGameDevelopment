using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Cripple : Effect
{
    private double storedMult = 0;
    private double newMult = 2;

    int updates = 0;
    void Effect.apply(Entity affected)
    {
        storedMult = affected.getDamageMult();
        affected.setDamageMult(newMult);
    }

    string Effect.ToString()
    {
        return "Crippled";
    }

    bool Effect.update(bool isPlayerTurn, Entity affected)
    {
        updates++;
        if(updates>=2)
        {
            affected.setDamageMult(storedMult);
            return true;
        }
        return false;
    }
}

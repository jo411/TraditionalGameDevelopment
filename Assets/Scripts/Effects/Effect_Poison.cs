using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Poison : Effect
{
    int turns = 0;
    int totalTurns = 4;
    double percentHP = .06;
    public bool update(bool isPlayerTurn, Entity affected)
    {

        affected.dealDamage(affected.getMaxHP() * percentHP, false, 1);
        turns++;
        if(turns==totalTurns)
        {
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return "Poison";
    }

   

    public void apply(Entity affected)
    {
        return;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Drain : Effect
{
    int initialHP;

    double percentReturned = 1.1;
    int duration=3;
    int turnsTaken=0;
    void Effect.apply(Entity affected)
    {
        initialHP = affected.getHP();
        affected.dealDamage(initialHP / 2,false,1);
    }

    string Effect.ToString()
    {
        return "Drained";
    }

    bool Effect.update(bool isPlayerTurn, Entity affected)
    {
        turnsTaken++;
        if(turnsTaken>duration)
        {
            if(!affected.isDead())
            {              
                affected.dealDamage(-1 * (initialHP * percentReturned), true, 1);
            }
            return true;
        }
       return false;
    }
}

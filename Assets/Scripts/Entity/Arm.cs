﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Arm class, player can have one left arm and one right arm
 * at a time unless otherwise stated. 
 */

// tell if left or right arm (isRight)
// contain Attack object for attack arm does
// contain Stats object for stats arm adds to the player

public class Arm
{

    public bool isRight;
    public Stats stats;
    public Attack attack;

    public Arm(bool isRight)
    {
        this.isRight = isRight;
        attack = AttackList.getRandomAttack();
        stats = new Stats(30);
    }

    
    public override string ToString()
    {
        return attack.ToString() + "\n\n" + "Stat Bonuses:\n" +stats.getStatString();
    }
}

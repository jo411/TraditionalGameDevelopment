using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

static class Calculator
{
    public static System.Random rand = new System.Random();

    public static double getHitProbability(double aBase, double targetEvasion)
    {
        return aBase * (.9 / targetEvasion);
    }

    public static bool checkProbability(double probability)
    {
        int chance = (int)(100 * probability);
        return rand.Next(101) <= chance;
    }
    public static double getDamage(int level, int attack, int targetDefense, int power, double critMod)
    {
        double levelTerm = (2 * level) / 5 + 2;

        double powerTerm = power * ((double)attack / targetDefense);

        return (((levelTerm * powerTerm) / (50)) + 2) * critMod;
    }



}



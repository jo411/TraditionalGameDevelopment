using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackVariance {

    public string name;
    public int minPower;
    public int maxPower;
    public double accuracy;
    public string flavorText;
    public Effect effect;
    public double effectChance;
    public AttackVariance(string name, int minPower, int maxPower, double accuracy, string text)
    {
        this.name = name;
        this.minPower = minPower;
        this.maxPower = maxPower;
        this.accuracy = accuracy;
        this.flavorText = text;
    }

    public AttackVariance(string name, int minPower, int maxPower, double accuracy, Effect effect, double effectChance, string text)
    {
        this.name = name;
        this.minPower = minPower;
        this.maxPower = maxPower;
        this.accuracy = accuracy;
        this.flavorText = text;
        this.effect = effect;
        this.effectChance = effectChance;
    }

    public int rollAttackPower()
    {
        return Random.Range(minPower, maxPower);
    }

    public override string ToString()
    {
        return "Attack: " + name + "\nminPower:" + minPower + "\nmaxPower:" + maxPower + "\nAccuracy:" + accuracy + "\n\n" + flavorText;
    }
}

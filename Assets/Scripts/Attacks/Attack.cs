using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public string name;
    public int power;
    public double accuracy;
    public string flavorText;
    public Effect effect;
    public double effectChance;
    public string sound;
    public Attack(string name, int power, double accuracy,string text,string sound)
    {
        this.name = name;
        this.power = power;
        this.accuracy = accuracy;
        this.flavorText = text;
        this.sound = sound;
    }

    public Attack(string name, int power, double accuracy, Effect effect, double effectChance ,string text, string sound)
    {
        this.name = name;
        this.power = power;
        this.accuracy = accuracy;
        this.flavorText = text;
        this.effect = effect;
        this.effectChance = effectChance;
        this.sound = sound;
    }

    public int rollAttackPower()
    {
        if(power == 0)
        {
            return power;
        }
        return Random.Range(power - 5, power + 5);
    }

    public override string ToString()
    {
       return "Attack: " + name + "\nPower:" + power + "\nAccuracy:" + accuracy + "\n\n" + flavorText;
    }

}


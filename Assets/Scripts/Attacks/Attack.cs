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
    public Attack(string name, int power, double accuracy,string text)
    {
        this.name = name;
        this.power = power;
        this.accuracy = accuracy;
        this.flavorText = text;
    }

    public Attack(string name, int power, double accuracy, Effect effect, double effectChance ,string text)
    {
        this.name = name;
        this.power = power;
        this.accuracy = accuracy;
        this.flavorText = text;
        this.effect = effect;
        this.effectChance = effectChance;
    }

    public override string ToString()
    {
       return "Attack: " + name + "\nPower:" + power + "\nAccuracy:" + accuracy + "\n\n" + flavorText;
    }

}


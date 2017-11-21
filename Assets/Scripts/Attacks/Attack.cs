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

    public int usesLeft;
    public int uses;
    public Attack(string name, int power, double accuracy,string text,string sound, int uses)
    {
        this.name = name;
        this.power = power;
        this.accuracy = accuracy;
        this.flavorText = text;
        this.sound = sound;
        this.usesLeft = uses;
        this.uses = uses;
    }

    public Attack(string name, int power, double accuracy, Effect effect, double effectChance ,string text, string sound, int uses)
    {
        this.name = name;
        this.power = power;
        this.accuracy = accuracy;
        this.flavorText = text;
        this.effect = effect;
        this.effectChance = effectChance;
        this.sound = sound;
        this.usesLeft = uses;
        this.uses = uses;
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
       return "Attack: " + name +"Uses: "+usesLeft+"\\"+uses+"\n"+ "\nPower:" + power + "\nAccuracy:" + accuracy + "\n\n" + flavorText;
    }

}


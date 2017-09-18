using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    string name;
    public int power;
    public double accuracy;
    public Attack(string name, int power, double accuracy)
    {
        this.name = name;
        this.power = power;
        this.accuracy = accuracy;
    }
}


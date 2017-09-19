using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    private string[] powerClass = new string[] { "Whelp", "Average", "Hero", "Legendary", "Godlike" };

    public int HP;
    public int attack;
    public int defense;
    public int speed;
    public double evasion;
    public int level;
    public double critChance = .5;

    private int maxValue = 255;
    public Stats()//default constructor generates random values
    {
        //super basic heuristic will weight lower stats if the previous was high and the other way around.
        //Should make all  stats somewhat balanced. But it tends to only create heroes because obviously its just an average......
        //HP = Calculator.rand.Next(1, maxValue+1);
        //attack = Calculator.rand.Next(maxValue+1 - HP, maxValue+1);
        //defense = Calculator.rand.Next(maxValue+1 - attack, maxValue+1);
        //speed = Calculator.rand.Next(maxValue + 1 - defense, maxValue + 1);


        HP = Calculator.rand.Next(1, maxValue + 1);
        attack = Calculator.rand.Next(1, maxValue + 1);
        defense = Calculator.rand.Next(1, maxValue + 1);
        speed = Calculator.rand.Next(1, maxValue + 1);



        evasion = Calculator.rand.Next(1, 101) / 100.0;
        level = Calculator.rand.Next(1, 101);
        level = 20;


    }
    public Stats(int HP, int attack, int defense, int speed, double evasion, int level)
    {
        this.HP = HP;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
        this.evasion = evasion;
        this.level = level;
    }

    public int getClassInt()
    {
        int sum = statSum();
        int index = 0;
        if (sum <= 255)
        {
            index = 0;
        }
        else if (sum <= 510)
        {
            index = 1;
        }
        else if (sum <= 765)
        {
            index = 2;
        }
        else if (sum <= 1020)
        {
            index = 3;
        }
        else
        {
            index = 4;
        }

        return index;
    }

    public string getClassString()
    {
       

        return powerClass[getClassInt()];
    }

    public int statSum()
    {
        return this.HP + this.attack + this.defense + this.speed;
    }
    public override string ToString()
    {
        return "Rank: " + getClassString() + "\n"
            + "Level: " + level + "\n"
           + "HP: " + HP + "\n"
            + "Attack: " + attack + "\n"
             + "Defense: " + defense + "\n"
              + "Speed: " + speed + "\n"
               + "Evasion: " + evasion + "\n";
    }
}


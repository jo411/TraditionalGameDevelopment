using System;
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
    public double incomingDamageMult=1;
   

    private int maxValue = 255;
    public Stats(int statTotal)//
    {

        List<double> distribution = getDistribution();

        HP = (int)(distribution[0] * statTotal);
        attack = (int)(distribution[1] * statTotal);
        defense= (int)(distribution[2] * statTotal);
        speed = (int)(distribution[3] * statTotal);

        evasion = Calculator.rand.Next(1, 101) / 100.0;      
        level = 20;      


        //super basic heuristic will weight lower stats if the previous was high and the other way around.
        ////Should make all  stats somewhat balanced. But it tends to only create heroes because obviously its just an average......
        //HP = Calculator.rand.Next(1, maxValue + 1);
        //attack = Calculator.rand.Next(maxValue + 1 - HP, maxValue + 1);
        //defense = Calculator.rand.Next(maxValue + 1 - attack, maxValue + 1);
        //speed = Calculator.rand.Next(maxValue + 1 - defense, maxValue + 1);


        //HP = Calculator.rand.Next(1, maxValue + 1);
        //attack = Calculator.rand.Next(1, maxValue + 1);
        //defense = Calculator.rand.Next(1, maxValue + 1);
        //speed = Calculator.rand.Next(1, maxValue + 1);



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

    /// <summary>
    /// Calculate a random stat distribution
    /// </summary>
    /// <returns></returns>
    public List<double> getDistribution()
    {
        List<double> percents = new List<double>();
        double[] nums = new double[5];
        nums[0] = 0;
        nums[4] = 100;


        for(int i=1;i<=3;i++)
        {
            nums[i] = Calculator.rand.Next(0, 101);
        }

        Array.Sort(nums);

        double sum = 0;
        for(int i=0;i<4;i++)
        {
            percents.Add( (nums[i + 1] - nums[i]) / 100);
            sum += percents[i];
        }
        
        
        return ShuffleList(percents);//reorder which stats are which
    }

    private List<E> ShuffleList<E>(List<E> inputList)
    {
        List<E> randomList = new List<E>();

       
        int randomIndex = 0;
        while (inputList.Count > 0)
        {
            randomIndex = Calculator.rand.Next(0, inputList.Count); //Choose a random object in the list
            randomList.Add(inputList[randomIndex]); //add it to the new, random list
            inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        }

        return randomList; //return the new random list
    }
}


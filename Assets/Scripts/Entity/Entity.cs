﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Entity: MonoBehaviour
{
  

    public List<Arm> arms;
 

    public List<Effect> effects;
    private Image healthBar;
    public GameObject combatText;

    // Use this for initialization
    void Start()
    {
        healthBar = transform.Find("EntityCanvas").Find("HealthBG").Find("Health").GetComponent<Image>();
    
       
    }


    public GameObject initCBT(string text)//TODO: a lot of finds and get components consider prefetching
    {
        GameObject temp = Instantiate(combatText) as GameObject;
        RectTransform tempRect = temp.GetComponent<RectTransform>();
        temp.transform.SetParent(transform.Find("EntityCanvas"));
        tempRect.transform.localPosition = combatText.transform.localPosition;
        tempRect.transform.localScale = combatText.transform.localScale;
        tempRect.transform.localRotation = combatText.transform.localRotation;

        temp.GetComponent<Text>().text = text;

        Destroy(temp.gameObject, 2);
        return temp;

    }
    public void addAttacks()
    {
        for(int i =0; i<2; i++)
        {
            arms.Add(new Arm());
        }
    }
    // Update is called once per frame
    void Update()
    {
       
    }

    private Stats baseStats;
    Stats stats;
    public string eName;
   
    public void Initialize(string name, int statTotal)//calls constructor
    {
        arms = new List<Arm>();
        effects = new List<Effect>();
        addAttacks();
        noParamEntity(name,statTotal);
    }
    public void noParamEntity(String name, int statTotal)
    {
        this.baseStats = new Stats(statTotal);//random stats
        this.eName = name;
        resetTempStats();
    }
   
    public void  parameterEntity(String name, Stats stats)
    {
        this.baseStats = stats;
        this.eName = name;
        resetTempStats();
    }

    /// <summary>
    /// Takes the target and an attack and tries to execute it.
    /// </summary>
    /// <param name="target">The entity being hit</param>
    /// <param name="attack">The attack being used</param>
    /// <returns>A tuple containing a string description of the attack with details seperated by newlines and a double of the damage dealth</String></returns>
    public AttackResult attackOther(Entity target, Attack attack)
    {
        
        List<string> log = new List<string>();
        bool hit;
        bool crit=false;
        double critMult = 1.5;//TODO: store this elsewhere
        log.Add(this.eName + " attacks " + target.eName + "!\n");

        
        double hitChance = Calculator.getHitProbability(attack.accuracy, target.getEvasion());
        double damage = 0;
        if (!Calculator.checkProbability(hitChance))
        {
            log.Add("The attack missed!");
            hit = false;
            damage = 0;
        }
        else
        {
            hit = true;
            double critMod = Calculator.checkProbability(stats.critChance) ? critMult : 1;
            crit= critMod !=1;
            string hitString = critMod == 1 ? "hits" : "crits";

            // switch to true to turn off attack variance
            if(false)
            {
                int power = attack.rollAttackPower();
                print("base power: " + attack.power + "| actual power: " + power);
                damage = Calculator.getDamage(stats.level, getAttack(), target.getDefense(), power, critMod);
            } else {
                damage = Calculator.getDamage(stats.level, getAttack(), target.getDefense(), attack.power, critMod);
            }
            
            if(Calculator.checkProbability(attack.effectChance))
            {
                Effect newEffect = (Effect)Activator.CreateInstance(attack.effect.GetType());
                target.effects.Add(newEffect);
                newEffect.apply(target);
            }



            log.Add(eName + " " + hitString + " for " + (int)damage + " damage!");
          
        }      
        target.dealDamage(damage,crit,critMult);
        //target.hitUI(damage, crit, critMult);


        return new AttackResult(log, hit,crit,damage);
    }

    public void turnTick(bool isPlayerTurn)
    {
        
      for(int i=effects.Count-1;i>=0;i--)
        {
            if (effects[i].update(isPlayerTurn, this))
            {
                effects.RemoveAt(i);
            }
        }
       
    }
    public Attack chooseAttack()
    {
        return arms[Calculator.rand.Next(0, arms.Count)].attack;//just use a random attack for now
    }

    public void setTempStats(Stats baseStats)
    {
        this.stats = new Stats(baseStats.HP, baseStats.attack, baseStats.defense, baseStats.speed, baseStats.evasion, baseStats.level);//set up temp stats for a battle
    }
    public void resetTempStats()//Reset the temp stats to base
    {
        this.stats = new Stats(this.baseStats.HP, this.baseStats.attack, this.baseStats.defense, this.baseStats.speed, this.baseStats.evasion, baseStats.level);//set up temp stats for a battle
    }

    public override string ToString()
    {
        return "Name: " + this.eName + "\n"
            +"Status: "+getStatusStrings()+"\n"
            + "Stats: " + "\n"
            + this.baseStats;

    }
    public string getStatusStrings()
    {
        StringBuilder sb = new StringBuilder();
        foreach(Effect current in effects)
        {
            sb.Append(" " + current.ToString());
        }
        return sb.ToString();
    }
    public void dealDamage(double damage, bool crit, double mult)
    {
        damage *= stats.incomingDamageMult;
        stats.HP -= (int)damage;
        if (stats.HP < 0)
        {
            stats.HP = 0;
        }else if(stats.HP>baseStats.HP)
        {
            stats.HP = baseStats.HP;
        }
        this.hitUI(damage, crit, mult);
    }

    public void hitUI(double damage, bool crit, double mult)
    {
        healthBar.fillAmount = (float)stats.HP / (float)baseStats.HP;

        if(damage==0)
        {
            initCBT("Miss").GetComponent<Animator>().SetTrigger("Hit");//TODO: double check the output format
            return;
        }
        if(damage<0)
        {
            initCBT("Healed: "+ damage*-1).GetComponent<Animator>().SetTrigger("Crit");//TODO: double check the output format
            return;
        }
        if (crit)
        {
            initCBT(damage.ToString("##0.##") + " (x" + mult.ToString() + ")").GetComponent<Animator>().SetTrigger("Crit");//TODO: double check the output format
        }
        else
        {
            initCBT(damage.ToString("##0.##")).GetComponent<Animator>().SetTrigger("Hit");//TODO: double check the output format
        }

    }
    public bool isDead()
    {
        return getHP() == 0;//if no hp this entity is dead
    }

    //These methods will allow for adding modifiers to any stat 
    public int getHP()
    {
        return stats.HP;
    }
    public int getMaxHP()
    {
        return baseStats.HP;
    }
    public int getAttack()
    {
        return stats.attack;
    }
    public int getDefense()
    {
        return stats.defense;
    }
    public int getSpeed()
    {
        return stats.speed;
    }

    public double getEvasion()
    {
        return stats.evasion;
    }

    public void setDamageMult(double value)
    {
        stats.incomingDamageMult = value;
    }
    public double getDamageMult()
    {
        return stats.incomingDamageMult;
    }

    public void setDefense(int value)
    {
        stats.defense = value;
    }
    public void setHP (int value)
    {
        stats.HP = Math.Min(getMaxHP(), value);
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackResult {
   public List<string> log;
    bool hit;
    bool crit;
    double damage;

    public AttackResult(List<string> log, bool hit, bool crit, double damage)
    {
        this.log = log;
        this.hit = hit;
        this.crit = crit;
        this.damage = damage;
    }
}


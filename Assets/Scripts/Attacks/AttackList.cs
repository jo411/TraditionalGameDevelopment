using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class AttackList {

    private static List<Attack> attacks = new List<Attack>()
    {
        new Attack("Fast Attack", 30, .95,"A weak but accurate attack","WeakAttack",8),
        new Attack("Strong Attack", 55, .6,"A strong but inaccurate attack","StrongAttack",4),
        new Attack("Poison Strike", 0, .9,new Effect_Poison(),1,"This attack applies a poison to the target.","FireDot",4),
        new Attack("Crippling Blow", 50, .8,new Effect_Cripple(),.8,"Bash the target with a mighty blow that may leave them crippled. A crippled target takes double damage during the next turn.","CripplingBlow",4),
        new Attack("Unholy Siphon",0,.7,new Effect_Drain(),1,"Drain the life force from the target dealing 1/2 their current hp. If the target is not slain before the effect dissapates they will be restored slightly more hp than they started with.","DrainLife",3)
    };

    
    public static Attack getRandomAttack()
    {
        return attacks[Calculator.rand.Next(0, attacks.Count)];
    }

    public static Attack getDefaultAttack()
    {
        return new Attack("Struggle", 40, .20, "A weak inaccurate attack", "WeakAttack", 8);
    }

    public static Attack getAttackAtIndex(int index)
    {
        if(index<attacks.Count-1&&index>=0)
        {
            return attacks[index];
        }
        else
        {
            return attacks[0];
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMeleeAttack : TAction{
    int creaturedice;
    int targetdice;
    int creaturemod;
    int targetmod;
    int threshold;
    public LightMeleeAttack(float range, GameObject boundCreature, string actionName) : base(range, boundCreature, actionName){}
    public override void Execute(){
        creaturedice = boundStats.agi + boundStats.LWSkill;
        targetdice = targetStats.agi + targetStats.lightDef;
        creaturemod = targetStats.agi;
        targetmod = targetStats.agi;
        threshold = targetStats.lightDef;
        int damage =  RollManager.Instance.RollContested(creaturedice,creaturemod,targetdice,targetmod,threshold);
        Debug.Log("Rolled damage = " + damage);
        targetStats.TakeDamage(damage);
        StopTargeting();
        if (boundStats.controlled)
        {
            boundCreature.GetComponent<PlayerAction>().actionsleft -= 1;
        }
        else
        {
            boundCreature.GetComponent<AutoAction>().actionsleft -= 1;
        }
    }
}

public class MediumMeleeAttack : TAction{
    int creaturedice;
    int targetdice;
    int creaturemod;
    int targetmod;
    int threshold;
    public MediumMeleeAttack(float range, GameObject boundCreature, string actionName) : base(range, boundCreature, actionName) { }
    public override void Execute()
    {
        if (boundStats.agi >= boundStats.str)
            creaturedice = boundStats.agi + boundStats.MWSkill;
        else
            creaturedice = boundStats.str + boundStats.MWSkill;

        targetdice = targetStats.agi + targetStats.medDef;
        creaturemod = 0;
        targetmod = 0;
        threshold = targetStats.medDef;
        int damage = RollManager.Instance.RollContested(creaturedice, creaturemod, targetdice, targetmod, threshold);
        Debug.Log("Rolled damage = " + damage);
        targetStats.TakeDamage(damage);
        StopTargeting();
        if (boundStats.controlled)
        {
            boundCreature.GetComponent<PlayerAction>().actionsleft -= 1;
        }
        else
        {
            boundCreature.GetComponent<AutoAction>().actionsleft -= 1;
        }
    }
}

public class HeavyMeleeAttack : TAction{
    int creaturedice;
    int targetdice;
    int creaturemod;
    int targetmod;
    int threshold;
    public HeavyMeleeAttack(float range, GameObject boundCreature, string actionName) : base(range, boundCreature, actionName) { }
    public override void Execute()
    {
        creaturedice = boundStats.str + boundStats.HWSkill;
        targetdice = targetStats.agi + targetStats.heavyDef;
        creaturemod = boundStats.str;
        targetmod = targetStats.str;
        threshold = targetStats.heavyDef;
        int damage = RollManager.Instance.RollContested(creaturedice, 0, targetdice, 0, threshold);
        Debug.Log("Rolled damage = " + damage);
        targetStats.TakeDamage(damage);
        StopTargeting();
        if (boundStats.controlled)
        {
            boundCreature.GetComponent<PlayerAction>().actionsleft -= 1;
        }
        else
        {
            boundCreature.GetComponent<AutoAction>().actionsleft -= 1;
        }
    }
}

public class LightRangedAttack : TAction
{
    int creaturedice;
    int targetdice;
    int creaturemod;
    int targetmod;
    int threshold;
    public LightRangedAttack(float range, GameObject boundCreature, string actionName) : base(range, boundCreature, actionName) { }
    public override void Execute()
    {
        creaturedice = boundStats.str + boundStats.LRWSkill;
        targetdice = targetStats.agi + targetStats.lightDef;
        creaturemod = boundStats.agi;
        targetmod = targetStats.agi;
        threshold = targetStats.lightDef;
        int damage = RollManager.Instance.RollContested(creaturedice, 0, targetdice, 0, threshold);
        Debug.Log("Rolled damage = " + damage);
        targetStats.TakeDamage(damage);
        StopTargeting();
        if (boundStats.controlled)
        {
            boundCreature.GetComponent<PlayerAction>().actionsleft -= 1;
        }
        else
        {
            boundCreature.GetComponent<AutoAction>().actionsleft -= 1;
        }
    }
}

public class HeavyRangedAttack : TAction
{
    int creaturedice;
    int targetdice;
    int creaturemod;
    int targetmod;
    int threshold;
    public HeavyRangedAttack(float range, GameObject boundCreature, string actionName) : base(range, boundCreature, actionName) { }
    public override void Execute()
    {
        creaturedice = boundStats.str + boundStats.HRWSkill;
        targetdice = targetStats.str + targetStats.heavyDef;
        creaturemod = boundStats.str;
        targetmod = targetStats.str;
        threshold = targetStats.heavyDef;
        int damage = RollManager.Instance.RollContested(creaturedice, 0, targetdice, 0, threshold);
        Debug.Log("Rolled damage = " + damage);
        targetStats.TakeDamage(damage);
        StopTargeting();
        if (boundStats.controlled)
        {
            boundCreature.GetComponent<PlayerAction>().actionsleft -= 1;
        }
        else
        {
            boundCreature.GetComponent<AutoAction>().actionsleft -= 1;
        }
    }
}
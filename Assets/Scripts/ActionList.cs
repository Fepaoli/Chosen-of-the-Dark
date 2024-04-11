using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attack : TAction{
    public int attackDice;
    public int attackMod;
    public int type;
    public Attack(float range, GameObject boundCreature) : base(range, boundCreature){}

    public void ChangeAttackType(int attD, int attM, int attT){
        attackDice = attD;
        attackMod = attM;
        type = attT;
    }
    public override void Execute(){
        int damage = 0;
        if (type == 0)
            damage = RollManager.Instance.RollContested(boundStats.agi + boundStats.LWSkill,0,targetStats.agi + targetStats.lightDef,0);
        if (type == 1)
            damage = RollManager.Instance.RollContested(boundStats.str + boundStats.MWSkill,0,targetStats.agi + targetStats.medDef,0);
        if (type == 2)
            damage = RollManager.Instance.RollContested(boundStats.str + boundStats.HWSkill,0,targetStats.agi + targetStats.heavyDef,0);
        Debug.Log("Rolled damage = " + damage);
        targetStats.TakeDamage(damage);
        StopTargeting();
        boundCreature.GetComponent<PlayerAction>().actionsleft -= 1;
    }
}

public class LightMeleeAttack : TAction{
    public int attackDice;
    public int attackMod;
    public int type;
    public LightMeleeAttack(float range, GameObject boundCreature) : base(range, boundCreature){}

    public void ChangeAttackType(int attD, int attM, int attT){
        attackDice = attD;
        attackMod = attM;
        type = attT;
    }
    public override void Execute(){
        int damage = 0;
        if (type == 0)
            damage = RollManager.Instance.RollContested(boundStats.agi + boundStats.LWSkill,0,targetStats.agi + targetStats.lightDef,0);
        if (type == 1)
            damage = RollManager.Instance.RollContested(boundStats.str + boundStats.MWSkill,0,targetStats.agi + targetStats.medDef,0);
        if (type == 2)
            damage = RollManager.Instance.RollContested(boundStats.str + boundStats.HWSkill,0,targetStats.agi + targetStats.heavyDef,0);
        Debug.Log("Rolled damage = " + damage);
        targetStats.TakeDamage(damage);
        StopTargeting();
        boundCreature.GetComponent<PlayerAction>().actionsleft -= 1;
    }
}

public class MediumMeleeAttack : TAction{
    public int attackDice;
    public int attackMod;
    public int type;
    public MediumMeleeAttack(float range, GameObject boundCreature) : base(range, boundCreature){}

    public void ChangeAttackType(int attD, int attM, int attT){
        attackDice = attD;
        attackMod = attM;
        type = attT;
    }
    public override void Execute(){
        int damage = 0;
        if (type == 0)
            damage = RollManager.Instance.RollContested(boundStats.agi + boundStats.LWSkill,0,targetStats.agi + targetStats.lightDef,0);
        if (type == 1)
            damage = RollManager.Instance.RollContested(boundStats.str + boundStats.MWSkill,0,targetStats.agi + targetStats.medDef,0);
        if (type == 2)
            damage = RollManager.Instance.RollContested(boundStats.str + boundStats.HWSkill,0,targetStats.agi + targetStats.heavyDef,0);
        Debug.Log("Rolled damage = " + damage);
        targetStats.TakeDamage(damage);
        StopTargeting();
        boundCreature.GetComponent<PlayerAction>().actionsleft -= 1;
    }
}

public class HeavyMeleeAttack : TAction{
    public int attackDice;
    public int attackMod;
    public int type;
    public HeavyMeleeAttack(float range, GameObject boundCreature) : base(range, boundCreature){}

    public void ChangeAttackType(int attD, int attM, int attT){
        attackDice = attD;
        attackMod = attM;
        type = attT;
    }
    public override void Execute(){
        int damage = 0;
        if (type == 0)
            damage = RollManager.Instance.RollContested(boundStats.agi + boundStats.LWSkill,0,targetStats.agi + targetStats.lightDef,0);
        if (type == 1)
            damage = RollManager.Instance.RollContested(boundStats.str + boundStats.MWSkill,0,targetStats.agi + targetStats.medDef,0);
        if (type == 2)
            damage = RollManager.Instance.RollContested(boundStats.str + boundStats.HWSkill,0,targetStats.agi + targetStats.heavyDef,0);
        Debug.Log("Rolled damage = " + damage);
        targetStats.TakeDamage(damage);
        StopTargeting();
        boundCreature.GetComponent<PlayerAction>().actionsleft -= 1;
    }
}
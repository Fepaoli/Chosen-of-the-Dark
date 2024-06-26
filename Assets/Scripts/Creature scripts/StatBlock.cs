using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBlock : MonoBehaviour
{
    public bool controlled = true;
    public int str;
    public int agi;
    public int wit;
    public int emp;
    public GameObject creature;
    public List<Traits> traits;
    public List<TAction> actions;

    public int LWSkill;
    public int MWSkill;
    public int HWSkill;
    public int LRWSkill;
    public int HRWSkill;
    public int DMSkill;
    public int OMSkill;
    public int lightDef;
    public int medDef;
    public int heavyDef;
    public int baseSpeed;
    public int HP;
    public int currentHP;
    public int WP;
    public int currentWP;
    public int baseStam;
    public int stamina;
    public int staminaLeft;
    public int RolledInitiative;
    // Start is called before the first frame update
    void Start()
    {
        creature = gameObject;
        actions = new List<TAction>();
        SecondaryCalcs();
        currentHP = HP;
        currentWP = WP;
        staminaLeft = stamina;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RollInitiative(){
        RolledInitiative = RollManager.Instance.RollToDC((int)agi/2+2,(int)agi/2,10);
    }

    public void SpawnOnBattlefield(Vector2Int newCoords){
        creature.GetComponent<SpriteRenderer>().enabled = true;
        creature.GetComponent<CapsuleCollider2D>().enabled = true;
        creature.GetComponent<Pathfinder>().coords = newCoords;
        //MapController.Instance.map[newCoords].overlay.moveState = OverlayController.TileState.Occupied;
        creature.GetComponent<Transform>().position = MapController.Instance.GridToWorld(newCoords);
    }

    public void Despawn (){
        creature.GetComponent<SpriteRenderer>().enabled = false;
        creature.GetComponent<CapsuleCollider2D>().enabled = false;
    }
    public void SecondaryCalcs(){
        HP = 8 + 2*str;
        WP = 5 + 2*wit + 2*emp;
        stamina = baseStam + (int)Math.Truncate(str/4D) + (int)Math.Truncate(agi/4D);
    }

    public void AddAction(TAction newAction)
    {
        actions.Add(newAction);
    }

    public void RemoveAction(TAction newAction)
    {
        actions.Remove(newAction);
    }

    public void TakeDamage(int amount){
        currentHP -= amount;
        if (currentHP<=0){
            currentHP = 0;
            InitiativeController.Instance.KillCreature(gameObject);
        }
    }

    public void HealDamage(int amount){
        currentHP += amount;
        if (currentHP>=HP)
            currentHP = HP;
    }
}

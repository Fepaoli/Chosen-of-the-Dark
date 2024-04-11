using System.Collections;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class InitiativeController : MonoBehaviour
{
    private static InitiativeController ICinstance;
    public static InitiativeController Instance
    {
        get
        {
            if (ICinstance == null)
            {
                Debug.Log("No initiative?");
            }
            return ICinstance;
        }
    }

    public List<GameObject> InitiativeOrder;
    public GameObject currentActor;
    public int actorIndex;
    public Transform parent;
    //Action test

    private void Awake()
    {
        ICinstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform;
        actorIndex = 0;
        StateManager.Instance.OnBattleStart.AddListener(RollInitiative);
        StateManager.Instance.OnRoundStart.AddListener(NextInInitiative);
    }

    void RollInitiative(){
        // Get enemies
        foreach (Transform child in parent.GetChild(0)){
            child.gameObject.SetActive(true);
            InitiativeOrder.Add(child.gameObject);
            child.gameObject.GetComponent<StatBlock>().RollInitiative();
        }
        // Get party
        foreach (Transform child in parent.GetChild(1)){
            child.gameObject.SetActive(true);
            InitiativeOrder.Add(child.gameObject);
            child.gameObject.GetComponent<StatBlock>().RollInitiative();
            child.gameObject.GetComponent<StatBlock>().AddAction(new Attack(1.5F, child.gameObject));
        }

        // Finalize initiative order
        InitiativeOrder.Sort((s1,s2) => s2.GetComponent<StatBlock>().RolledInitiative.CompareTo(s1.GetComponent<StatBlock>().RolledInitiative));
        actorIndex = 0;
    }

    public bool IsActing(GameObject creature)
    {
        if (creature == currentActor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void NextInInitiative()
    {
        CursorController.Instance.Deselect();
        if (actorIndex == InitiativeOrder.Count)
        {
            actorIndex = 0;
            StateManager.Instance.UpdateState(StateList.newround);
        }
        bool auto = false;
        currentActor = InitiativeOrder[actorIndex];
        if (currentActor.GetComponent<StatBlock>().controlled)
        {
            StateManager.Instance.UpdateState(StateList.goodTurn);
            currentActor.GetComponent<PlayerAction>().TurnReset();
        }
        else
        {
            StateManager.Instance.UpdateState(StateList.evilTurn);
            currentActor.GetComponent<AutoAction>().TakeTurn();
            auto = true;
        }
        actorIndex++;
        if (auto)
        {
            NextInInitiative();
        }
    }
    public void SetupPathfinding(){
        // Get enemies
        foreach (Transform child in parent.GetChild(0)){
            child.gameObject.SetActive(true);
            child.gameObject.GetComponent<Pathfinder>().InitMap();
            InitiativeOrder.Add(child.gameObject);
            child.gameObject.GetComponent<StatBlock>().RollInitiative();
        }
        // Get party
        foreach (Transform child in parent.GetChild(1)){
            child.gameObject.SetActive(true);
            child.gameObject.GetComponent<Pathfinder>().InitMap();
            InitiativeOrder.Add(child.gameObject);
            child.gameObject.GetComponent<StatBlock>().RollInitiative();
        }
    }
}

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
            damage = RollManager.Instance.RollContested(boundStats.agi + boundStats.LWSkill,boundStats.agi,targetStats.agi + targetStats.lightDef,targetStats.agi);
        if (type == 1)
            damage = RollManager.Instance.RollContested(boundStats.str + boundStats.MWSkill,boundStats.agi,targetStats.agi + targetStats.medDef,targetStats.agi);
        if (type == 2)
            damage = RollManager.Instance.RollContested(boundStats.str + boundStats.HWSkill,boundStats.str,targetStats.agi + targetStats.heavyDef,targetStats.agi);
        Debug.Log("Rolled damage = " + damage);
        targetStats.TakeDamage(damage);
        StopTargeting();
        boundCreature.GetComponent<PlayerAction>().actionsleft -= 1;
    }
}
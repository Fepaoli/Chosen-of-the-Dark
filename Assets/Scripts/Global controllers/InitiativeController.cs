using System.Collections;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using UnityEditor.Experimental.GraphView;

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
            //Placeholder: manually assigns attacks to all party members
            string name = child.gameObject.name;
            if (name == "Priest")
                child.gameObject.GetComponent<StatBlock>().AddAction(new LightMeleeAttack(1.5F, child.gameObject,"Mace"));
            else if (name == "General")
                child.gameObject.GetComponent<StatBlock>().AddAction(new HeavyMeleeAttack(1.5F, child.gameObject,"Greatsword"));
            else if (name == "Hunter"){
                child.gameObject.GetComponent<StatBlock>().AddAction(new LightMeleeAttack(1.5F, child.gameObject,"Shortsword"));
                child.gameObject.GetComponent<StatBlock>().AddAction(new LightRangedAttack(7.5F, child.gameObject,"Hunting bow"));
            }
            else if (name == "Bureaucrat"){
                child.gameObject.GetComponent<StatBlock>().AddAction(new MediumMeleeAttack(1.5F, child.gameObject,"Longsword"));
            }
            else if (name == "Hero")
            {
                child.gameObject.GetComponent<StatBlock>().AddAction(new MediumMeleeAttack(1.5F, child.gameObject,"Warhammer"));
                child.gameObject.GetComponent<StatBlock>().AddAction(new HeavyRangedAttack(1.5F, child.gameObject,"Longbow"));
            }

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


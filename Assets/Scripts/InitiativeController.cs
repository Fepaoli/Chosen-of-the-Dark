using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
            InitiativeOrder.Add(child.gameObject);
            child.gameObject.GetComponent<StatBlock>().RollInitiative();
        }
        // Get party
        foreach (Transform child in parent.GetChild(1)){
            InitiativeOrder.Add(child.gameObject);
            child.gameObject.GetComponent<StatBlock>().RollInitiative();
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
}

public class Attack : IAction{
    GameObject attackTarget;
    StatBlock targetStats;
    GameObject boundCreature;
    StatBlock boundStats;
    public void StartTargeting(){
        CursorController.Instance.targeting = true;
        CursorController.Instance.currentAction = this;
    }
    public void GetTarget(GameObject target){
        attackTarget = target;
        targetStats = target.GetComponent<StatBlock>();
        Execute();
    }
    public void Execute(){
        int damage = RollManager.Instance.RollContested(boundStats.HWSkill,boundStats.agi,targetStats.HWSkill,targetStats.agi);
        //Call damage function on target
        CursorController.Instance.targeting = false;
        CursorController.Instance.currentAction = null;
    }

    public void BindActor (GameObject actor){
        boundCreature = actor;
        boundStats = boundCreature.GetComponent<StatBlock>();
    }
}
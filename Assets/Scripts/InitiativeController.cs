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
        StateManager.Instance.OnRoundStart.AddListener(RefreshAction);
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

    void RefreshAction(){
        foreach (GameObject creature in InitiativeOrder){
            if (creature.GetComponent<StatBlock>().controlled){
                creature.GetComponent<PlayerAction>().TurnReset();
            }
        }
        StateManager.Instance.UpdateState(StateList.newround);
        NextInInitiative();
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
        if (actorIndex == InitiativeOrder.Count)
        {
            actorIndex = 0;
            StateManager.Instance.UpdateState(StateList.newround);
        }
        else
        {
            bool auto = false;
            currentActor = InitiativeOrder[actorIndex];
            if (currentActor.GetComponent<StatBlock>().controlled)
            {
                StateManager.Instance.UpdateState(StateList.goodTurn);
            }
            else
            {
                StateManager.Instance.UpdateState(StateList.evilTurn);
                //currentActor.GetComponent<AutoAction>().TakeTurn();
                auto = true;
            }
            actorIndex++;
            if (auto)
            {
                NextInInitiative();
            }
        }
        
    }
}

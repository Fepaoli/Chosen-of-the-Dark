using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class InitiativeController : MonoBehaviour
{
    public List<GameObject> InitiativeOrder;
    public Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform;
        StateManager.Instance.OnBattleStart.AddListener(RollInitiative);
        StateManager.Instance.OnRoundStart.AddListener(RefreshAction);
    }

    // Update is called once per frame
    void RollInitiative(){
        // Get enemies
        foreach (Transform child in parent.GetChild(0)){
            InitiativeOrder.Add(child.gameObject);
            StatBlock init = child.gameObject.GetComponent<StatBlock>();
            init.RollInitiative();
        }
        // Get party
        foreach (Transform child in parent.GetChild(1)){
            InitiativeOrder.Add(child.gameObject);
            StatBlock init = child.gameObject.GetComponent<StatBlock>();
            init.RollInitiative();
        }

        // Finalize initiative order
        InitiativeOrder.Sort((s1,s2) => s1.GetComponent<StatBlock>().RolledInitiative.CompareTo(s2.GetComponent<StatBlock>().RolledInitiative));
    }

    void RefreshAction(){
        foreach (GameObject creature in InitiativeOrder){
            if (creature.GetComponent<StatBlock>().controlled){
                StateManager.Instance.UpdateState(StateList.goodTurn);
                creature.GetComponent<PlayerAction>().enabled = true;
                creature.GetComponent<PlayerAction>().TurnReset();
            }
            else{
                StateManager.Instance.UpdateState(StateList.evilTurn);
            }
        }
        StateManager.Instance.UpdateState(StateList.newround);
    }
}

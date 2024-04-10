using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class StateManager : MonoBehaviour
{
    public UnityEvent OnMenuReturn;
    public UnityEvent OnMenuExit;
    public UnityEvent OnBattleStart;
    public UnityEvent OnRoundStart;
    public UnityEvent OnTurnStart;
    public UnityEvent OnEnemyTurnStart;
    public UnityEvent OnBattleEnd;


    public Canvas UI;
    public GameObject map;

    public GameObject creatures;

    public static IState currentState;
    public static StateList stateType;
    private static StateManager SMinstance;
    public static StateManager Instance
    {
        get
        {
            if (SMinstance == null)
            {
                Debug.Log("No manager?");
            }
            return SMinstance;
        }
    }

    private void Awake()
    {
        SMinstance = this;
    }
    void Start()
    {
        currentState = new MainMenu();
        currentState.EnterState();
    }

    void Update(){
        if (stateType != currentState.GetType()){
            currentState.ExitState();
            switch(stateType){
                case StateList.menu:
                    currentState = new MainMenu();
                    currentState.EnterState();
                    break;
                case StateList.battleEnd:
                    currentState = new Playend();
                    currentState.EnterState();
                    break;
                case StateList.newround:
                    Debug.Log("Starting round!");
                    currentState = new RoundStart();
                    currentState.EnterState();
                    break;
                case StateList.battleStart:
                    Debug.Log("Starting battle!");
                    currentState = new Playstart();
                    currentState.EnterState();
                    break;
                case StateList.goodTurn:
                    currentState = new PlayerTurn();
                    currentState.EnterState();
                    break;
                case StateList.evilTurn:
                    currentState = new EnemyTurn();
                    currentState.EnterState();
                    break;
                default:
                    currentState = new MainMenu();
                    currentState.EnterState();
                    break;
            }
        }
    }

    public void UpdateState(StateList state)
    {
        stateType = state;
    }
}

public class MainMenu : IState
{
    public StateList stateType = StateList.menu;
    void IState.EnterState(){
        StateManager.Instance.OnMenuReturn.Invoke();
    }
    void IState.ExitState(){
        StateManager.Instance.OnMenuExit.Invoke();
    }
    StateList IState.GetType(){
        return stateType;
    }
}

public class RoundStart : IState
{
    public StateList stateType = StateList.newround;
    void IState.EnterState(){
        StateManager.Instance.creatures.SetActive(true);
        StateManager.Instance.OnRoundStart.Invoke();
    }
    void IState.ExitState(){

    }
    StateList IState.GetType(){
        return stateType;
    }
}
public class Playstart : IState
{
    public StateList stateType = StateList.battleStart;
    void IState.EnterState(){
        StateManager.Instance.map.SetActive(true);
        StateManager.Instance.OnBattleStart?.Invoke();
    }
    void IState.ExitState(){

    }
    StateList IState.GetType(){
        return stateType;
    }
}

public class Playend : IState
{
    public StateList stateType = StateList.battleEnd;
    void IState.EnterState(){
        StateManager.Instance.OnBattleEnd.Invoke();
    }
    void IState.ExitState(){

    }
    StateList IState.GetType(){
        return stateType;
    }
}

public class PlayerTurn : IState
{
    public StateList stateType = StateList.goodTurn;
    void IState.EnterState(){
        StateManager.Instance.OnTurnStart.Invoke();
    }
    void IState.ExitState(){

    }

    StateList IState.GetType(){
        return stateType;
    }
}


public class EnemyTurn : IState
{
    public StateList stateType = StateList.evilTurn;

    void IState.EnterState(){
        StateManager.Instance.OnEnemyTurnStart.Invoke();
    }
    void IState.ExitState(){

    }
    StateList IState.GetType(){
        return stateType;
    }
}

public enum StateList{
    menu,
    battleStart,
    newround,
    goodTurn,
    evilTurn,
    battleEnd
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class StateManager : MonoBehaviour
{
    public Canvas UI;
    public static IState currentState;
    public delegate void SwitchTo (StateList state);
    public static event SwitchTo ChangeState;
    public StateList stateType;
    // Start is called before the first frame update
    void Start()
    {
        currentState = new MainMenu();
        currentState.EnterState();
    }

    void Update(){
        if (stateType != currentState.GetType()){
            switch(stateType){
                case StateList.menu:
                    currentState = new MainMenu();
                    currentState.EnterState();
                    break;
                case StateList.battleEnd:
                    currentState = new Playend();
                    currentState.EnterState();
                    break;
                case StateList.battleStart:
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
}

public class MainMenu : IState
{
    public StateList stateType = StateList.menu;
    void IState.EnterState(){

    }
    void IState.SwitchState(IState switchTo){

    }
    StateList IState.GetType(){
        return stateType;
    }
}

public class Playstart : IState
{
    public StateList stateType = StateList.battleStart;
    void IState.EnterState(){

    }
    void IState.SwitchState(IState switchTo){

    }
    StateList IState.GetType(){
        return stateType;
    }
}

public class Playend : IState
{
    public StateList stateType = StateList.battleEnd;
    void IState.EnterState(){

    }
    void IState.SwitchState(IState switchTo){

    }
    StateList IState.GetType(){
        return stateType;
    }
}

public class PlayerTurn : IState
{
    public StateList stateType = StateList.goodTurn;
    void IState.EnterState(){

    }
    void IState.SwitchState(IState switchTo){

    }

    StateList IState.GetType(){
        return stateType;
    }
}


public class EnemyTurn : IState
{
    public StateList stateType = StateList.evilTurn;

    void IState.EnterState(){

    }
    void IState.SwitchState(IState switchTo){

    }
    StateList IState.GetType(){
        return stateType;
    }
}

public enum StateList{
    menu,
    battleStart,
    goodTurn,
    evilTurn,
    battleEnd
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBtn : MonoBehaviour
{
    public Button btn;
    public GameObject actor;
    public TAction linkedAction;
    public bool pressed = false;
    void Awake()
    {
        btn = gameObject.GetComponent<Button>();
    }
    public void LinkButton(GameObject creature, TAction action){
        actor = creature;
        linkedAction = action;
        btn.onClick.AddListener(SelectButton);
    }

    public void MoveButton(Vector3 coords){
        gameObject.transform.position = coords;
    }
    public void ResetButton(){
        pressed = false;
    }
    public void SelectButton(){
        if (pressed){
            linkedAction.StopTargeting();
            CursorController.Instance.targeting = false;
            pressed = false;
        }
        else{
            if (actor.GetComponent<PlayerAction>().actionsleft >0){
                pressed = true;
                linkedAction.StartTargeting();
                CursorController.Instance.targeting = true;
            }
        }
    }
}
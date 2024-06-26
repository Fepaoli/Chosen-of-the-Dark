using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == "Action name")
                child.gameObject.GetComponent<TMP_Text>().text = linkedAction.actionName;
            else
                child.gameObject.GetComponent<TMP_Text>().text = linkedAction.actionDescription;
        }
    }

    public void MoveButton(Vector3 coords){
        gameObject.GetComponent<RectTransform>().localPosition = coords;
    }
    public void ResetButton(){
        pressed = false;
    }
    public void SelectButton(){
        if (!CursorController.Instance.acting){
            if (pressed){
                linkedAction.StopTargeting();
                CursorController.Instance.targeting = false;
                pressed = false;
            }   
            else{
                if (actor.GetComponent<PlayerAction>().actionsleft>0){
                    pressed = true;
                    linkedAction.StartTargeting();
                    CursorController.Instance.targeting = true;
                    CursorController.Instance.ShowActionRange(linkedAction, actor.GetComponent<Pathfinder>());
                }
            }
        }
        
    }
}
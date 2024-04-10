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
    void Start()
    {
        btn = gameObject.GetComponent<Button>();
    }
    public void LinkButton(GameObject actor, TAction action){
        linkedAction = action;
        btn.onClick.AddListener(SelectButton);
    }

    void SelectButton(){
        if (pressed){
            linkedAction.StopTargeting();
            pressed = false;
        }
        else{
            linkedAction.StartTargeting();
            pressed = true;
        }
    }
}
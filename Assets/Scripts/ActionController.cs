using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    StatBlock selectedCharacter;
    public GameObject buttonTemplate;
    public void ShowActions(GameObject creature)
    {
        selectedCharacter = creature.GetComponent<StatBlock>();
        Debug.Log(selectedCharacter.actions);
        foreach (TAction action in selectedCharacter.actions){
            GameObject newButton = Instantiate(buttonTemplate, gameObject.transform);
            newButton.GetComponent<ActionBtn>().LinkButton(creature,action);
        }
    }

    public void HideActions()
    {
        
    }
}

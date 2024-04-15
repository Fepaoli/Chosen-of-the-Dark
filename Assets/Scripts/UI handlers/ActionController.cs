using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    StatBlock selectedCharacter;
    public GameObject buttonTemplate;
    public void ShowActions(GameObject creature)
    {
        Debug.Log("Executing button creation algorithm");
        Vector3 buttonPosition = new Vector3(0, -50, 0);
        selectedCharacter = creature.GetComponent<StatBlock>();
        foreach (TAction action in selectedCharacter.actions){
            Debug.Log("Repeating cyle");
            GameObject newButton = Instantiate(buttonTemplate, gameObject.transform, false);
            newButton.GetComponent<ActionBtn>().LinkButton(creature,action);
            newButton.GetComponent<ActionBtn>().MoveButton(buttonPosition);
            //Add attack dice number display
            buttonPosition += new Vector3(0, -30, 0);
        }
    }

    public void HideActions()
    {
        foreach (Transform child in transform){
            if (child.GetComponent<ActionBtn>() != null){
                Destroy(child.gameObject);
            }
            //Also hide text displaying attack dice
        }
    }
}

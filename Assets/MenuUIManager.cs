using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StateManager.Instance.OnMenuReturn.AddListener(Appear);
        StateManager.Instance.OnMenuExit.AddListener(Disappear);
        Disappear();
    }

    // Update is called once per frame
    void Appear(){
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    void Disappear(){
        gameObject.GetComponent<Canvas>().enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class StateChangeButton : MonoBehaviour
{
    public UIDocument document;
    public StateList tiedState;
    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn = new Button { text = "Next turn" };
        btn.clicked += SwitchState;

        document.rootVisualElement.Add(btn);
    }

    void SwitchState()
    {
        StateManager.Instance.UpdateState(tiedState);

    }
}

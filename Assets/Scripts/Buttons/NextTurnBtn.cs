using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class NextTurnBtn : MonoBehaviour
{
    public UIDocument document;
    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn = new Button { text = "Next turn" };
        btn.clicked += GoNext;

        document.rootVisualElement.Add(btn);
    }

    void GoNext()
    {
        InitiativeController.Instance.NextInInitiative();
    }
}

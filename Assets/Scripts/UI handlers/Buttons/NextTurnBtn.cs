using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NextTurnBtn : MonoBehaviour
{
    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(GoNext);
    }

    void GoNext()
    {
        if (!CursorController.Instance.acting)
            InitiativeController.Instance.NextInInitiative();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StateChangeButton : MonoBehaviour
{
    public StateList tiedState;
    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(SwitchState);
    }

    void SwitchState()
    {
        StateManager.Instance.UpdateState(tiedState);

    }
}

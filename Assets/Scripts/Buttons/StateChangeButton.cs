using System.Collections;
using System.Collections.Generic;
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
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SwitchState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchState()
    {
        StateManager.Instance.UpdateState(tiedState);

    }
}

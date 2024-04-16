using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    private static BattleUIManager BMInstance;
    public static BattleUIManager Instance
    {
        get
        {
            if (BMInstance == null)
            {
                Debug.Log("No Battle UI?");
            }
            return BMInstance;
        }
    }
    void Awake(){
        BMInstance = this;
    }
    public GameObject creatureInspector;
    public GameObject creatureName;
    public GameObject creatureStats;
    public GameObject terrainName;
    public GameObject terrainStats;
    public GameObject terrainInspector;
    public GameObject turnVisualizer;
    // public ActionController buttons;
    // Start is called before the first frame update
    void Start()
    {
        creatureName = creatureInspector.transform.GetChild(0).gameObject;
        creatureStats = creatureInspector.transform.GetChild(1).gameObject;
        StateManager.Instance.OnBattleStart.AddListener(Appear);
        StateManager.Instance.OnBattleEnd.AddListener(Disappear);
        DeselectCreature();
        Disappear();
    }

    // Update is called once per frame
    void Appear(){
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    void Disappear(){
        gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void DeselectCreature()
    {
        creatureStats.SetActive(false);
        creatureName.SetActive(false);
        creatureInspector.SetActive(false);
    }

    public void InspectCreature(GameObject creature){
        gameObject.transform.GetChild(0).GetComponent<ActionController>().HideActions();
        creatureStats.SetActive(true);
        creatureName.SetActive(true);
        creatureInspector.SetActive(true);
        Debug.Log("Inspecting creature");
        creatureName.GetComponent<TMP_Text>().SetText(creature.name);
        StatBlock stats = creature.GetComponent<StatBlock>();
        string stamina = "";
        for (int i = 0; i<=stats.stamina;i++)
        {
            if (i<=stats.staminaLeft)
                stamina += "O";
            else
                stamina += "|";
        }
        creatureStats.GetComponent<TMP_Text>().SetText("HP: " + stats.currentHP + "/" + stats.HP + "\nDef = " + stats.lightDef + "(L) " + stats.medDef + "(M) " + stats.heavyDef + "(H)\n Speed = " + (stats.agi + stats.baseSpeed));
        if (creature.GetComponent<StatBlock>().controlled){
            gameObject.transform.GetChild(0).GetComponent<ActionController>().ShowActions(creature);
        }
    }

    public void InspectTerrain(GameObject terrain){
        terrainName.GetComponent<TMP_Text>().SetText(terrain.name);
    }

    public void UpdateCreatureInspector(bool show){
        creatureInspector.SetActive(show);
    }

    public void UpdateTerrainInspector(bool show){
        terrainInspector.SetActive(show);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageScroller : MonoBehaviour
{
    private List<GameObject> panels;
    // Start is called before the first frame update

    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel4;
    public GameObject panel5;
    public GameObject panel6;
    public GameObject panel7;
    public GameObject panel8;
    public GameObject panel9;
    public GameObject panel10;

    public Button goNext;
    public Button goPrev;

    public int index;
    void Start()
    {
        panels = new List<GameObject>() { panel1, panel2, panel3, panel4, panel5, panel6, panel7, panel8, panel9, panel10 };
        goNext.onClick.AddListener(ShowNext);
        goPrev.onClick.AddListener(ShowPrev);
    }

    // Update is called once per frame
    public void ShowNext()
    {
        if (index < panels.Count - 1 )
        {
            panels[index].SetActive(false);
            index++;
            panels[index].SetActive(true);
        }
    }

    public void ShowPrev()
    {
        if (index > 0)
        {
            panels[index].SetActive(false);
            index--;
            panels[index].SetActive(true);
        }
    }
}

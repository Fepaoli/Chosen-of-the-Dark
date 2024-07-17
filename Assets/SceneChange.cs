using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneChange : MonoBehaviour
{
    public Button btn;
    void Start()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(SwitchScene);
    }

    void SwitchScene()
    {
        SceneManager.LoadScene("UI example scene");
    }
}

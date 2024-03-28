using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    void Update()
    {
        if (CursorController.Instance.isOnUI)
        {
            Debug.Log("overUI");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorController.Instance.isOnUI = true;
        Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorController.Instance.isOnUI = false;
        Debug.Log("Mouse exit");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorController.Instance.isOnUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorController.Instance.isOnUI = false;
    }
}
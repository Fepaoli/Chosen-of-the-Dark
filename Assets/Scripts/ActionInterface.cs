using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    void StartTarget();
    void StopTarget();
    void Execute(GameObject target);
    void Undo();
    void SetSprite (Sprite icon);
}

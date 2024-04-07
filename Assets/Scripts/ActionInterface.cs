using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    void StartTargeting();
    void GetTarget(GameObject target);
    void Execute();
    void BindActor(GameObject actor);
}

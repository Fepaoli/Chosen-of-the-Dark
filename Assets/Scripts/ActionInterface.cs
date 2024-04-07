using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
<<<<<<< Updated upstream
    void StartTarget();
    void StopTarget();
    void Execute(GameObject target);
    void Undo();
    void SetSprite (Sprite icon);
}
=======
    void StartTargeting();
    void GetTarget(GameObject target);
    void Execute();
    void BindActor(GameObject actor);
}
>>>>>>> Stashed changes

using UnityEngine;

/*
    Script: EnemyController
    Author: Gareth Lockett
    Version: 1.0
    Description:    Simple enemy movement script that other enemy controllers inherit from.
                    Sets target on inherited controllers.
*/

public abstract class EnemyController : CharacterController
{
    // Methods
    public virtual void SetTarget( GameObject targetGameObject ){} // Enemy calls this when they want the enemy controller to target an object (eg player)
}

using UnityEngine;

/*
    Script: EnemyControllerAimer
    Author: Gareth Lockett
    Version: 1.0
    Description:    Simple enemy aimer script (eg stationary guns/cannons)
                    Add this to an enemy object you want to control.
                    Will aim along to forward axis and use Vector3.up as up direction.
*/

public class EnemyControllerAimer : EnemyController
{
    // Properties
    public float aimingSpeed = 3f;          // Speed which to smoothly aim at the target object.

    public bool xAxisLimit, yAxisLimit, zAxisLimit; // Limit the various axis.

    private GameObject targetObject;        // Target object this enemy will aim at.

    // Methods
    private void Update()
    {
        // Check if there is anything to aim at.
        if( this.targetObject == null ){ return; }

        // Aim at the target object.
        Quaternion rot = this.transform.rotation;
        this.transform.LookAt( this.targetObject.transform.position, Vector3.up );

        // Smooth the aim.
        this.transform.rotation = Quaternion.Slerp( rot, this.transform.rotation, Time.deltaTime *this.aimingSpeed );

        // Check axis limits.
        Vector3 eulerAngles = this.transform.eulerAngles;
        if( this.xAxisLimit == true ){ eulerAngles.x = 0f; }
        if( this.yAxisLimit == true ){ eulerAngles.y = 0f; }
        if( this.zAxisLimit == true ){ eulerAngles.z = 0f; }
        this.transform.eulerAngles = eulerAngles;
    }

    public override void SetTarget( GameObject targetGameObject )
    {
        // Set the target object.
        this.targetObject = targetGameObject;
    }
}

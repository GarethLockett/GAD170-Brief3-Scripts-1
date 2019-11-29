using UnityEngine;

/*
    Script: EnemyControllerAimer
    Author: Gareth Lockett
    Version: 1.0
    Description:    Simple enemy aimer script (eg stationary guns/cannons)
                    Add this to an enemy object you want to control.
                    Will aim along to forward axis and use Vector3.up as up direction.
                    Attach a weapon to this game object and reference here to fire when within an angle.

*/

public class EnemyControllerAimer : EnemyController
{
    // Properties
    public float aimingSpeed = 3f;                  // Speed which to smoothly aim at the target object.
    public bool xAxisLimit, yAxisLimit, zAxisLimit; // Limit the various axis.

    public float targetAngleToFire = 5f;            // Angle to target before calling Fire() on weapon.
    public ItemWeapon weapon;                       // Reference to a weapon component (eg attach a weapon to this game object and reference here!)

    private GameObject targetObject;                // Target object this enemy will aim at.

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

        // Check angle for firing.
        if( this.weapon != null )
        {
            // Calculate the vector from this game object to the target object.
            Vector3 vecToTarget = ( this.targetObject.transform.position - this.transform.position ).normalized;

            // Check the angle between this game object forward vector and the calculated vector from this game object to target game object.
            if( Vector3.Angle( this.transform.forward, vecToTarget ) <= this.targetAngleToFire )
            {
                // Activate (eg fire the weapon) .. NOTE: the weapon should be taking care of fire rate, instantiating projectiles etc
                this.weapon.Activate( this.gameObject.GetComponent<Character>() );
            }
        }
    }

    public override void SetTarget( GameObject targetGameObject )
    {
        // Set the target object.
        this.targetObject = targetGameObject;
    }
}

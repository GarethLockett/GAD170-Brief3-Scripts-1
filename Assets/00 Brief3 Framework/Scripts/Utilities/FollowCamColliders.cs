using UnityEngine;

/*
    Script: FollowCamColliders
    Author: Gareth Lockett
    Version: 1.0
    Description: Used in conjunction with FollowCam. Checks for collision objects between the target object (on FollowCam) and the FollowCam object.
                 If a collision is found this script will move the FollowCam object closer to the target object.
                 Note: This component should be attached AFTER the FollowCam component.
*/

[ RequireComponent( typeof( FollowCam ) ) ]
public class FollowCamColliders : MonoBehaviour
{
    // Properties
    [ Range(0,10) ] public float adjustSpeed = 5f;      // If there is a collision, how fast to move the FollowCam to the collision position.
    [ Range(0,1) ] public float extraSpaceBuffer;       // Some extra space between the FollowCam and collider.
    public LayerMask collisionLayers;                   // The layers to raycast against.

    private FollowCam followCam;                        // Cache the reference to FollowCam (eg so don't have to keep using GetComponent every Update())

    // Methods
    private void Start()
    {
        this.followCam = this.GetComponent<FollowCam>();
    }

    private void Update()
    {
        // Sanity checks.
        if( this.followCam == null ){ return; }
        if( this.followCam.targetObj == null ){ return; }

        // Check for collisions between the target object and follow camera.
        RaycastHit hit;
        Vector3 vecFromTargetToFollowCam = this.followCam.transform.position - this.followCam.targetObj.transform.position;
        if( Physics.Raycast( this.followCam.targetObj.transform.position, vecFromTargetToFollowCam, out hit, vecFromTargetToFollowCam.magnitude, this.collisionLayers ) == true )
        {
            // Calculate the target FollowCam position.
            Vector3 pos = hit.point;

            // Add any buffer from the hit point.
            pos += -vecFromTargetToFollowCam.normalized *this.extraSpaceBuffer;

            // Smoothly move the FollowCam to the target position.
            this.followCam.transform.position = Vector3.Lerp( this.followCam.transform.position, pos, Time.deltaTime *this.adjustSpeed );
        }
    }
}

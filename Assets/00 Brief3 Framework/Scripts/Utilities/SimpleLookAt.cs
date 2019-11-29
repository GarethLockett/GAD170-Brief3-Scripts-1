using UnityEngine;

/*
    Script: SimpleLookAt
    Author: Gareth Lockett
    Version: 1.0
    Description:    Super simple script for making a game object look at another game object in the scene.
                    Objects look along their Z axis using world Y axis as up node.
                    Options for smooth rotation.
*/

public class SimpleLookAt : MonoBehaviour
{
    // Properties
    public GameObject targetGameObject;     // The game object to look at.
    public bool smoothRotate;               // Toggle smooth rotate.
    public float rotationSpeed = 45f;       // Speed which to smoothly rotate at.

    // Methods
    private void Update()
    {
        // Sanity checks.
        if( this.targetGameObject == null ){ return; }

        // Rotate towards target game object.
        Quaternion currentRotation = this.transform.rotation;
        this.transform.LookAt( this.targetGameObject.transform.position, Vector3.up );

        // Check for smooth rotation.
        if( this.smoothRotate == false ){ return; }

        // Smooth rotate.
        this.transform.rotation = Quaternion.Slerp( currentRotation, this.transform.rotation, Time.deltaTime *this.rotationSpeed );
    }
}

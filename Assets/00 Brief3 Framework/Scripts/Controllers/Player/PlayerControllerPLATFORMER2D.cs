﻿using UnityEngine;

/*
    Script: PlayerControllerPLATFORMER2D
    Author: Gareth Lockett
    Version: 1.0
    Description:    Simple 2D platformer movement script.
                    Add this a player object you want to control.
                    Make sure player starts facing the direction you want to be 'forward'.
                    Handles player input and movement.
                    Uses physics. NOTE: Make sure to put a collider on this object or child objects!
*/

[ RequireComponent( typeof( Rigidbody ) ) ]
public class PlayerControllerPLATFORMER2D : CharacterController
{
    // Properties
    public bool usePhysicsMovement = false;             // Use physics instead of transform movement (eg vehicles etc)
    public float moveSpeed = 3f;                        // Player translation speed.
    public float jumpAmount = 5f;                       // Force amount to jump up.
    
    public KeyCode moveForwardKey = KeyCode.D;          // Move forward key.
    public KeyCode moveBackwardKey = KeyCode.A;         // Move backward key.
    public KeyCode jumpKey = KeyCode.Space;             // Jump upward key.

   // private bool doJumpNextFixedUpdate;                 // Helper to catch key press in Update() but execution in FixedUpdate()
    private Vector3 startingDirection;                  // Capture the starting direction so can switch back and forth as moving forwards and backwards.

    private Rigidbody rb;                               // The rigidbody on this game object.

    // Methods
    private void Start()
    {
        // Get the reference to the rigidbody on this game object.
        this.rb = this.GetComponent<Rigidbody>();

        // Make sure the rigidbody has constraints on rotation (So doesn't fall over)
        this.rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Get starting direction.
        this.startingDirection = this.transform.forward;
    }

    private void Update()
    {
        if( this.usePhysicsMovement == false )
        {
            // Check for player input and set move direction.
            Vector3 moveDirection = Vector3.zero;
            if( Input.GetKey( this.moveForwardKey ) == true ) { moveDirection += this.transform.forward = this.startingDirection; }
            if( Input.GetKey( this.moveBackwardKey ) == true ) { moveDirection += this.transform.forward = -this.startingDirection; }

            // Do the actual move forward by multiplying it by time and player movement speed.
            this.transform.position += moveDirection.normalized * Time.deltaTime * this.moveSpeed;
        }
    }

    // Physics calls should only be done in FixedUpdate()
    private void FixedUpdate()
    {
        if( this.usePhysicsMovement == true )
        {
            // Check player input and set move direction.
            Vector3 moveDirection = Vector3.zero;
            if( Input.GetKey( this.moveForwardKey ) == true ) { moveDirection += this.transform.forward = this.startingDirection; }
            if( Input.GetKey( this.moveBackwardKey ) == true ) { moveDirection += this.transform.forward = -this.startingDirection; }

            // Add some force to the rigidbody to move it.
            this.rb.AddForce( moveDirection.normalized * this.moveSpeed );
        }

        // Handle jumping.
        if( Input.GetKey( this.jumpKey ) == true )
        {
            // Check the character is standing on something (NOTE: May want to change this to detect what it is standing on)
            if( Physics.Raycast( this.transform.position +( Vector3.up *0.01f ), -Vector3.up, 0.02f ) == true )
            {
                // Use physics system to add some upward force.
               this.GetComponent<Rigidbody>().AddForce( Vector3.up *this.jumpAmount, ForceMode.Impulse );
            }
        }
    }
}

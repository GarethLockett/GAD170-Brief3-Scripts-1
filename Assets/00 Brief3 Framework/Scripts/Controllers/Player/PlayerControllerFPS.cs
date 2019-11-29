using UnityEngine;

/*
    Script: PlayerControllerFPS
    Author: Gareth Lockett
    Version: 1.0
    Description:    Simple first person shooter movement script.
                    Add this a player object you want to control.
                    Make sure there is an active child camera if you want mouse look.
                    Handles player input and movement.
                    Uses physics. Could be wise to add a capsule collider to this game object (Make sure to position/size appropriately) In case the child objects don't have any.
                    Press ESC key to exit cursor lock.
*/

[ RequireComponent( typeof( Rigidbody ) ) ]
public class PlayerControllerFPS : CharacterController
{
    // Properties
    public bool usePhysicsMovement = false;             // Use physics instead of transform movement (eg vehicles etc)
    public float moveSpeed = 3f;                        // Player translation speed.
    public float turnSpeed = 60f;                       // Player rotation speed.
    public float jumpAmount = 5f;                       // Force amount to jump up.

    public KeyCode moveForwardKey = KeyCode.W;          // Move forward key.
    public KeyCode moveBackwardKey = KeyCode.S;         // Move backward key.
    public KeyCode rotateLeftKey = KeyCode.A;           // Rotate left key.
    public KeyCode rotateRightKey = KeyCode.D;          // Rotate right key.
    public KeyCode jumpKey = KeyCode.Space;             // Jump upward key.

    private Camera childCamera;                         // This will contain a reference to a child camera (If has one)
    
    private Rigidbody rb;                               // The rigidbody on this game object.

    // Methods
    private void Start()
    {
        // Will grab a reference to a child camera. If it does not have one (or camera/GameObject is disabled) it will not do camera mouse look.
        this.childCamera = this.GetComponentInChildren<Camera>();

        // Chek if child camera is disabled or hidden in the hierarchy.
        if( this.childCamera != null )
            { if( this.childCamera.gameObject.activeInHierarchy == false || this.childCamera.enabled == false ){ this.childCamera = null; } }
        
        // Hide/lock the cursor if has a child camera (eg FPS)
        if( this.childCamera != null ){ Cursor.lockState = CursorLockMode.Locked; }

        // Get the reference to the rigidbody on this game object.
        this.rb = this.GetComponent<Rigidbody>();

        // Make sure the rigidbody has constraints on rotation (So doesn't fall over)
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        // Check player input and set move direction.
        Vector3 moveDirection = Vector3.zero;
        if( Input.GetKey( this.moveForwardKey ) == true ){ moveDirection += this.transform.forward; }
        if( Input.GetKey( this.moveBackwardKey ) == true ){ moveDirection -= this.transform.forward; }
    
        // Check if we have a child camera for mouse look (FPS) or else use left/right keys to rotate.
        if( this.childCamera != null )
        {
            // Get the amount the mouse has moved since last Update()
            Vector2 mouseDelta = Vector2.zero;
            float mouseSensitivity = 10f; // Play around with mouse look speed
            mouseDelta.x = Input.GetAxis( "Mouse X" ) *mouseSensitivity;
            mouseDelta.y = Input.GetAxis( "Mouse Y" ) *mouseSensitivity *2f;

            // Rotate this game object around its' Y axis (For the horizontal mouse look)
            this.transform.Rotate( this.transform.up, Time.deltaTime *this.turnSpeed *mouseDelta.x );
            
            // Rotate the child camera using the mouse Y delta by moving the forward vector up or down.
            Vector3 forwardVec = this.childCamera.transform.forward;
            forwardVec.y += Time.deltaTime *this.turnSpeed *( mouseDelta.y / Screen.height );
            this.childCamera.transform.forward = forwardVec;

            // Strafe by adding to the current move direction.
            if( Input.GetKey( this.rotateLeftKey ) == true ){ moveDirection -= this.transform.right; }
            if( Input.GetKey( this.rotateRightKey ) == true ){ moveDirection += this.transform.right; }
        }
        else
        {
            if( Input.GetKey( this.rotateLeftKey ) == true ){ this.transform.Rotate( this.transform.up, Time.deltaTime *-this.turnSpeed ); }
            if( Input.GetKey( this.rotateRightKey ) == true ){ this.transform.Rotate( this.transform.up, Time.deltaTime *this.turnSpeed ); }
        }

        // Do the actual move by normalizing (eg make a 'unit vector') the move direction and then multiply it by time and player movement speed.
        if( this.usePhysicsMovement == false ){ this.transform.position += moveDirection.normalized * Time.deltaTime * this.moveSpeed; }

        // Press escape key to show cursor again (eg If FPS)
        if( Input.GetKey( KeyCode.Escape ) == true ){ Cursor.lockState = CursorLockMode.None; }

    }
    
    // Physics calls should only be done in FixedUpdate()
    private void FixedUpdate()
    {
        // Check player input and set move direction.
        Vector3 moveDirection = Vector3.zero;
        if( Input.GetKey( this.moveForwardKey ) == true ) { moveDirection += this.transform.forward; }
        if( Input.GetKey( this.moveBackwardKey ) == true ) { moveDirection -= this.transform.forward; }

        // Strafe by adding to the current move direction.
        if( this.childCamera != null )
        {
            if( Input.GetKey( this.rotateLeftKey ) == true ) { moveDirection -= this.transform.right; }
            if( Input.GetKey( this.rotateRightKey ) == true ) { moveDirection += this.transform.right; }
        }

        // Add some force to the rigidbody to move it.
        if( this.usePhysicsMovement == true ){ this.rb.AddForce( moveDirection.normalized * this.moveSpeed ); }

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

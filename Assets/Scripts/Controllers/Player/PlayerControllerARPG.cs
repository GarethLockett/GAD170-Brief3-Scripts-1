using UnityEngine;
using UnityEngine.AI;

/*
    Script: PlayerControllerARPG
    Author: Gareth Lockett
    Version: 1.0
    Description:    Simple third person movement script.
                    Add this a player object you want to control.
                    Handles player input (mouse click)
                    Uses physics. Uses physics. Could be wise to add a capsule collider to this game object (Make sure to position/size appropriately) In case the child objects don't have any.
                    Uses navigation mesh (NavMeshAgent) *NOTE: Make sure to have baked navigation mesh in the scene!
*/

[ RequireComponent( typeof( Rigidbody ) ) ]
[ RequireComponent( typeof( NavMeshAgent ) ) ]
public class PlayerControllerARPG : CharacterController
{
    // Events
    public delegate void ObjectClicked( GameObject clickedGameObject );
    public ObjectClicked objectClicked; // Other classes can subscribe to this to receive clicked objects other than the ground (eg GameManager)

    // Properties
    public float moveSpeed = 3f;                        // Player translation speed.
    
    public LayerMask groundLayers;     // The layers containing either the ground layers the player can move on.

    private Camera activeCamera;        // Reference to the camera to raycast from for mouse clicks.
    private NavMeshAgent navMeshAgent;  // Referent to the NavMeshAgent component on this game object.

    // Methods
    private void Start()
    {
        // Get the main camera. ** NOTE: If you are going to have multiple camera in the scene then you may need to change this!
        this.activeCamera = Camera.main;

        // Get a reference to the NavMeshAgent on this game object.
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.navMeshAgent.speed = this.moveSpeed;
    }

    private void OnDisable()
    {
        if( this.navMeshAgent != null ){ this.navMeshAgent.ResetPath(); }
    }

    private void Update()
    {
        // Check for a left mouse click (0)
        if( Input.GetMouseButton( 0 ) == true )
        {
            // Doublic check there is still a camera to raycast from.
            if( this.activeCamera == null ){ Debug.LogWarning( "No camera found in the scene to raycast from!" ); this.enabled = false; return; }

            // Get a ray from the mouse click point (In camera) into the 3D scene.
            Ray ray = this.activeCamera.ScreenPointToRay( Input.mousePosition );

            // Raycast from the camera into the scene.
            RaycastHit hit;

            // if( Physics.Raycast( ray, out hit, Mathf.Infinity, this.groundLayers ) == true )
            if( Physics.Raycast( ray, out hit, Mathf.Infinity ) == true )
            {
                // Unity has a complex way of testing if a LayerMask contains a specific layer! :P (eg the clicked objects layer)
                if( this.groundLayers.value == ( this.groundLayers.value | ( 1 << hit.collider.gameObject.layer ) ) )
                {
                    // Set the navMeshAgent destination to the hit point.
                    //this.navMeshAgent.SetDestination( hit.point );
                    this.SetNavMeshAgentDestination( hit.point );
                }
                else
                {
                    // Something other than the ground was clicked.
                    Debug.Log( "Something other than the ground was clicked! " +hit.collider.gameObject.name );
                    if( this.objectClicked != null ){ this.objectClicked( hit.collider.gameObject ); }
                }

                // // Set the navMeshAgent destination to the hit point.
                // this.navMeshAgent.SetDestination( hit.point );
            }
        }
    }

    public void SetNavMeshAgentDestination( Vector3 position )
    {
        this.navMeshAgent.SetDestination( position );
    }
}

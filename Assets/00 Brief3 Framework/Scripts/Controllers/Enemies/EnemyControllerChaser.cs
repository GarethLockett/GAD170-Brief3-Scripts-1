using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    Script: EnemyControllerChaser
    Author: Gareth Lockett
    Version: 1.0
    Description:    Simple enemy chaser script.
                    Add this to an enemy object you want to control.
                    Uses navigation mesh (NavMeshAgent)
                        *NOTE: Make sure to have baked navigation mesh in the scene!
                        Make sure to add some waypoints.
*/

[ RequireComponent( typeof( NavMeshAgent) ) ]
public class EnemyControllerChaser : EnemyController
{
    // Enumerators
    public enum EnemyControllerState{ _idle, _waypoints, _chase }

    // Properties
    public float moveSpeed = 3f;            // Player translation speed.

    public EnemyControllerState state;      // Keeps track of the state of this enemy.
    public List<Transform> waypoints;       // Waypoints for this enemy to move between.

    public float idleWaitTime = 5f;         // Number of seconds allowed to be idle before starting to move between waypoints.

    private NavMeshAgent navMeshAgent;      // Reference to this objects' NavMeshAgent.
    private float exitIdleTime;             // Time to leave 'idle' state and go into 'waypoints' state

    private GameObject targetObject;        // Target object this enemy will chase after.

    // Methods
    private void Start()
    {
        // Make sure the rigidbody has constraints on rotation (So doesn't fall over)
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        // Get a reference to the NavMeshAgent on this game object.
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.navMeshAgent.speed = this.moveSpeed;

        // Set first auto-waypoint time.
        this.exitIdleTime = Time.time +this.idleWaitTime;
    }

    private void OnDisable()
    {
        if( this.navMeshAgent != null ){ this.navMeshAgent.ResetPath(); }
    }

    private void Update()
    {
        // Do the current state of this enemy.
        switch( this.state )
        {
            case EnemyControllerState._idle:
                // Check if been idle long enough
                if( Time.time >= this.exitIdleTime ){ this.state = EnemyControllerState._waypoints; break; }

                // Shouldn't have a target object if not chasing.
                this.targetObject = null;

                // Shouldn't have a path if idle.
                this.navMeshAgent.ResetPath();
                break;

            case EnemyControllerState._waypoints:
                // Set a new random destination if not already heading to one.
                if( this.navMeshAgent.hasPath == false )
                {
                    // Clean out any empty waypoint elements.
                    this.waypoints.RemoveAll( item => item == null );

                    if( this.waypoints.Count > 0 )
                        { this.navMeshAgent.SetDestination( this.waypoints[ Random.Range( 0, this.waypoints.Count ) ].position ); }
                    else
                    {
                        this.state = EnemyControllerState._idle;
                        this.exitIdleTime = Time.time +this.idleWaitTime;
                    }
                }
                
                // Shouldn't have a target object if not chasing.
                this.targetObject = null;
                break;

            case EnemyControllerState._chase:
                // Check still got a target object or set back to idle.
                if( this.targetObject == null ){ this.state = EnemyControllerState._idle; break; }

                // Set navMeshAgent destination to target object position.
                this.navMeshAgent.SetDestination( this.targetObject.transform.position );
                break;
        }
    }

    public override void SetTarget( GameObject targetGameObject )
    {
        // Set the target object.
        this.targetObject = targetGameObject;

        // Check if there is a target object.
        if( this.targetObject == null )
        {
            if( this.state == EnemyControllerState._chase ){ this.state = EnemyControllerState._idle; } // Stop chasing if no target.
            return;
        }

        // Start chasing.
        this.state = EnemyControllerState._chase;
    }
}

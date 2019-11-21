using UnityEngine;

/*
    Script: CharacterEnemy
    Author: Gareth Lockett
    Version: 1.0
    Description:    Simple enemy script.
                    Interacts with an EnemyController (Make sure to add one or won't do much)
                    Searches for a player with attackRange to target.
*/

public class CharacterEnemy : Character
{
    // Enumerators
    protected enum EnemyState{ _idle, _targetingPlayer }    // Enemy states

    // Properties
    public float attackRange = 5f;          // If a CharacterPlayer is found within this range then will attack.
    // TODO: Fire within range/angle...

    protected EnemyState enemyState;             // Keep track of this enemies states.
    protected EnemyController controller;   // The enemy controller component on this game object.

    private CharacterPlayer targetPlayer;   // Player character to attack.

    // Methods
    private void Start()
    {
        // Try and get the enemy controller (Enemy can't do much without one!)
        this.controller = this.GetComponent<EnemyController>();
    }

    private void Update()
    {
        // Do enemy current state
        switch( this.enemyState )
        {
            case EnemyState._idle:
                // Can't do much without a enemy controller.
                if( this.controller == null ){ break; }

                // Make sure the controller isn't still targeting an object.
                this.controller.SetTarget( null );

                // Look for a player.
                CharacterPlayer characterPlayer = GameObject.FindObjectOfType<CharacterPlayer>();
                if( characterPlayer != null )
                {
                    // Check distance to player and target if within range.
                    if( Vector3.Distance( this.transform.position, characterPlayer.transform.position ) <= this.attackRange )
                    {
                        // Set the target player and state of this enemy.
                        this.targetPlayer = characterPlayer;
                        this.enemyState = EnemyState._targetingPlayer;

                        // Set the target object on the controller.
                        this.controller.SetTarget( this.targetPlayer.gameObject );
                    }
                }

                break;

            case EnemyState._targetingPlayer:
                // Check still have a valid player target.
                if( this.targetPlayer == null ){ this.enemyState = EnemyState._idle; break; }

                // Make sure player still in range.
                if( Vector3.Distance( this.transform.position, this.targetPlayer.transform.position ) > this.attackRange )
                {
                    this.targetPlayer = null; // Forget player.
                    this.enemyState = EnemyState._idle;
                    break;
                }

                break;
        }
    }
}

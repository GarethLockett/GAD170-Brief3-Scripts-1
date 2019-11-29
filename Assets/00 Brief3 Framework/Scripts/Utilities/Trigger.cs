using UnityEngine;

/*
    Script: Trigger
    Author: Gareth Lockett
    Version: 1.0
    Description:    Base class for triggering something when a character enters or exits a trigger.
*/

[ RequireComponent( typeof( Collider ) ) ]
public abstract class Trigger : MonoBehaviour
{
    // Enumerators
    public enum TriggerType{ _enter, _exit }

    // Properties
    public TriggerType type;                    // Either executes on either entering or exiting the trigger.
    public bool playerCanTrigger = true;        // Allow CharacterPlayers to trigger.
    public bool enemyCanTrigger = false;        // Allow CharacterEnemys to trigger.
    public bool disableOnTrigger;               // Should this script be disabled once the trigger has been triggered?

    // Methods
    private void Awake()
    {
        // Make sure the collider is set to trigger.
        this.gameObject.GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter( Collider collider )
    {
        // Trigger only if type _enter.
        if( this.type == TriggerType._enter ){ this.ActivateTrigger( collider ); }
    }
    private void OnTriggerExit( Collider collider )
    {
        // Trigger only if type _exit.
        if( this.type == TriggerType._exit ){ this.ActivateTrigger( collider ); }
    }

    protected virtual bool ActivateTrigger( Collider collider )
    {
        // Sanity check.
        if( collider == null ){ return false; }

        // Make sure a Character is trigger.
        if( collider.attachedRigidbody == null ){ return false; } // Some non-rigidbody is trigger??
        Character character = collider.attachedRigidbody.gameObject.GetComponent<Character>();
        if( character == null ){ return false; } // Some non-Character is trigger??

        // Check what kind of characters can trigger.
        if( this.playerCanTrigger == false || this.enemyCanTrigger == false )
        {
            if( this.playerCanTrigger == false )
            {
                // Check if character is a player. Don't continue if character is a player and playerCanTrigger is false.
                if( character as CharacterPlayer != null ){ return false; }
            }

            if( this.enemyCanTrigger == false )
            {
                // Check if character is an enemy. Don't continue if character is an enemy and enemyCanTrigger is false.
                if( character as CharacterEnemy != null ){ return false; }
            }
        }

        return true;
    }
}

using UnityEngine;

/*
    Script: DamageZone
    Author: Gareth Lockett
    Version: 1.0
    Description:    Simple script for damaging characters who enter / stay / exit a zone
                    Uses a trigger collider.
*/

[ RequireComponent( typeof( Collider ) ) ]
public class DamageZone : MonoBehaviour
{
    // Enumerators
    public enum TriggerType{ _stay, _enter, _exit }

    // Properties
    public TriggerType type;            // When to trigger (eg on stay within trigger, or when entering, or exiting)
    public float damageAmount = 1f;     // Amount of damage to do to a character per second (stay) or once per event (enter/exit)
    public bool destroyAfterTrigger;    // Destroy this game object after trigger damage done?


    // Methods
    private void Start()
    {
        // Check collider is a trigger.
        Collider collider = this.gameObject.GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter( Collider collider )
    {
        // Check type.
        if( this.type != TriggerType._enter ){ return; }

        // Check the collider has an attached rigidbody (eg Characters have to have a rigidbody!)
        if( collider.attachedRigidbody == null ){ return; }

        // Try and pass a Character component to EnterExitDoDamage()
        this.EnterExitDoDamage( collider.attachedRigidbody.gameObject.GetComponent<Character>() );
    }

    private void OnTriggerExit( Collider collider )
    {
        // Check type.
        if( this.type != TriggerType._exit ){ return; }

        // Check the collider has an attached rigidbody (eg Characters have to have a rigidbody!)
        if( collider.attachedRigidbody == null ){ return; }

        // Try and pass a Character component to EnterExitDoDamage()
        this.EnterExitDoDamage( collider.attachedRigidbody.gameObject.GetComponent<Character>() );
    }

    private void EnterExitDoDamage( Character character )
    {
        // Sanity check.
        if( character == null ){ return; }

        // Damage the character once on entering the trigger.
        character.TakeDamage( this.damageAmount );
//Debug.Log( character.gameObject.name +" took damage" +this.damageAmount +" from a DamageZone", this.gameObject );

        // Destroy the damage zone after it has done damage?
        if( this.destroyAfterTrigger == true ){ Destroy( this.gameObject ); }
    }

    private void OnTriggerStay( Collider collider )
    {
        // This works differently than OnTriggerEnter / OnTriggerExit. It scales the amount of damage based on how long the Character has been in the zone.

        // Check type.
        if( this.type != TriggerType._stay ){ return; }

        // Check the collider has an attached rigidbody (eg Characters have to have a rigidbody!)
        if( collider.attachedRigidbody == null ){ return; }

        // Get/check character.
        Character character = collider.attachedRigidbody.gameObject.GetComponent<Character>();
        if( character == null ){ return; }

        // Damage the character depending on delta time.
        character.TakeDamage( this.damageAmount *Time.deltaTime );
    }
}

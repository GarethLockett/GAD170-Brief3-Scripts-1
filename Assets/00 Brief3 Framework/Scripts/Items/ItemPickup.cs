using UnityEngine;

/*
    Script: ItemPickup
    Author: Gareth Lockett
    Version: 1.0
    Description: Base class for pickup/consumable items (eg health pack, gold, effect)
*/

public abstract class ItemPickup : Item
{
    // Properties
    public bool activateOnTrigger;  // Does the item automatically activate when triggered/picked up (eg consumables)

    // Methods
    public virtual void OnTriggerEnter( Collider collider )
    {
        // Check this should call Activate() if triggered.
        if( this.activateOnTrigger == false ){ return; }

        // Check the thing colliding with this has a rigidbody attached (eg character)
        if( collider.attachedRigidbody == null ){ return; }

        // Check a character is triggering this (eg could be a non-character, like a projectile instead)
        Character character = collider.attachedRigidbody.gameObject.GetComponent<Character>();
        if( character == null ){ return; }

        // Automatically Activate() on entering the trigger.
        this.Activate( character );
    }
}

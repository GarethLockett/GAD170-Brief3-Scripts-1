using UnityEngine;

/*
    Script: ItemPickupHealth
    Author: Gareth Lockett
    Version: 1.0
    Description:    Item pickup that adds a fixed amount of health to the character.
*/

public class ItemPickupHealth : ItemPickup
{
    // Properties
    public float amountOfHealthToAdd = 1f;      // Amount of health to add to the character.

    // Methods
    private void Start()
    {
        // Auto make sure the first collider is a trigger.
        Collider collider = this.GetComponent<Collider>();
        if( collider != null ){ collider.isTrigger = true; }
    }

    public override void Activate( Character characterActivating )
    {
        if( characterActivating == null ){ return; }

        // Check if player pick up.
        if( this.playerPickup == true && characterActivating.GetComponent<CharacterPlayer>() != null )
        {
            characterActivating.GainHealth( this.amountOfHealthToAdd ); // Add player health.
            Destroy( this.gameObject ); // Destroy the item once used.
            return;
        }

        // Check if enemy pick up.
        if( this.enemiesPickup == true && characterActivating.GetComponent<CharacterEnemy>() != null )
        {
            characterActivating.GainHealth( this.amountOfHealthToAdd ); // Add player health.
            Destroy( this.gameObject ); // Destroy the item once used.
            return;
        }

        // Extend to support other character types?...
    }
}

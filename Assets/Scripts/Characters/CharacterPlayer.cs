using UnityEngine;

/*
    Script: CharacterPlayer
    Author: Gareth Lockett
    Version: 1.0
    Description:    The player's main class.
                    Handles player input for activating the active item.
*/

public class CharacterPlayer : Character
{
    // Properties
    public bool autoEquipWeapons = true;    // Automatically equip any items that are picked up in OnTriggerEnter().

    public KeyCode activateKey;             // Key to press to Activate() the activeItem.
    public bool mouseActiveItem;            // If true will allow user to press the left mouse button to also Activate() the activeItem.

    // Methods
    private void Start()
    {
        // Register this player.
        GameManager.RegisterPlayer( this );
    }

    private void Update()
    {
        // Check for player pressing the fire key and/or the left mouse click (If mouseActiveItem is ticked on)
        if( Input.GetKey( this.activateKey ) == true || ( this.mouseActiveItem == true && Input.GetMouseButton( 0 ) == true ) )
        {
            // Call ActivateItem() in base class.
            this.ActivateItem();
        }
    }

    public override void GetItem( Item item )
    {
        // Sanity check.
        if( item == null ){ return; }

        // Handle weapon items.
        ItemWeapon weapon = item as ItemWeapon; // Try to 'cast' Item to type 'ItemWeapon' (Returns null if can't cast)
        if( weapon != null )
        {
            // Add the weapon to this characters' inventory.
            this.inventory.AddItem( weapon );

            // If autoEquipWeapons set to true, then equip the weapon (eg make active)
            if( this.autoEquipWeapons == true )
            {
                this.inventory.MakeActiveItem( weapon );
                weapon.EquipWeapon();
            }
        }

        // Handle pickup items.
        ItemPickup pickup = item as ItemPickup; // Try to 'cast' Item to type 'ItemWeapon' (Returns null if can't cast)
        if( pickup != null )
        {
            // Don't handle here if activateOnTrigger is true (eg will handle itself)
            if( pickup.activateOnTrigger == false )
            {
                // Add the item to the character inventory.
                this.inventory.AddItem( pickup );
            }
        } 
    }

    protected override void OnTriggerEnter( Collider collider )
    {
        // Handle item triggers.
        Item item = collider.gameObject.GetComponent<Item>();
        if( item != null )
        {
            // Check if a player can pick up the item.
            if( item.playerPickup == true )
            {
                // Handle the item pick up.
                this.GetItem( item );
            }
        }
    }

}
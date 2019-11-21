using UnityEngine;

/*
    Script: Item
    Author: Gareth Lockett
    Version: 1.0
    Description: Base class that all items inherit from.
*/

public abstract class Item : MonoBehaviour
{
    // Properties
    public string itemName;         // Name of the item.
    public string description;      // Short description of the item.

    public bool playerPickup = true;            // Players can pick this item up.
    public bool enemiesPickup = false;          // Enemies can pick this item up.
    // Extend to support other character types?...

    // Methods
    public abstract void Activate( Character characterActivating ); // Called whenever the item is 'activated' (eg weapon fired, potion drank etc)

    // Methods
    private void Awake()
    {
        // Register for any GameManager events (Note this should work even if there are no active GameManagers in the scene)
        GameManager.gameEnded += this.GameEnded;
    }

    protected virtual void GameEnded()
    {
        // Fix in case this game object was just destroyed.
        if( this == null ){ return; }

        // Clean up any character related things when the game ends.
        //Debug.Log( "GameEnded called on Character: " +this.gameObject.name );

        // Disable this character component at the end of the game so can't do anything else.
        this.enabled = false;
    }
}

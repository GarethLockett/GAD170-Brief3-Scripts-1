using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Script: Inventory
    Author: Gareth Lockett
    Version: 1.0
    Description:    A simple inventory class for characters.
                    Handles inventorying of items.
                    Adding / removing items.
                    Currently active inventory item.
*/

[ System.Serializable ] // Use this code to show the Inventory class in the Unity Editor Inspector.
public class Inventory
{
    // Properties
    public List<Item> items = new List<Item>();     // List of all the items in this inventory. *NOTE: Leave public if you want designers to pre-populate with items (Scene game objects NOT prefabs!)
    private Item activeItem;                        // Currently active item.

    //public Transform transform;                     // This should be set to the characters' gameObject (eg done in Character)
    private Character character;                    // Reference to the character this inventory is on.

    // Methods
    public void Init( Character character ){ this.character = character; }

    public void AddItem( Item item )
    {
        // Sanity check.
        if( item == null ){ return; }

        // Check item isn't already in this inventory (eg Don't add the same item twice)
        if( this.items.Contains( item ) == true ){ return; }

        // Add the item to the items list.
        this.items.Add( item );

        // Hide the item in the hierarchy and make child of this game object.
        this.DisableItem( item );
    }

    public void RemoveItem( Item item )
    {
        // Sanity check.
        if( item == null ){ return; }

        // Remove if active item (Set another item as active?)
        if( this.activeItem == item )
        {
            // Drop the item.
            this.activeItem.transform.SetParent( null, true );

            // Clear the active item.
            this.activeItem = null;
        }

        // Remove the item from the items list.
        this.items.Remove( item );
    }

    public void DestroyItem( Item item )
    {
        // Sanity check.
        if( item == null ){ return; }

        // Remove if active item (Set another item as active?)
        if( this.activeItem == item ){ this.activeItem = null; }

        // Destroy the game object.
        GameObject.Destroy( item.gameObject );
    }

    public void MakeActiveItem( Item item )
    {
        // If item is null, then disable active item and set activeItem to null.
        if( item == null )
        {
            this.DisableItem( this.activeItem );
            this.activeItem = null;
            return;
        }

        // Check item is in the items list, if not, automatically add it.
        if( this.items.Contains( item ) == false )
        {
            // Add the item to the items list.
            this.items.Add( item );
        }

        // If there is another active item already in use, disable it.
        if( item != this.activeItem ){ this.DisableItem( this.activeItem ); }

        // Make item the active item.
        this.activeItem = item;

        // Enable the item.
        this.EnableItem( this.activeItem );
    }

    private void EnableItem( Item item )
    {
        // Sanity check.
        if( item == null ){ return; }

        // Attach the item to the characters' itemAttachPoint (eg hand)
        if( this.character.itemAttachPoint != null )
        {
            item.transform.SetParent( this.character.itemAttachPoint, false );
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }
        else
        {
            // Where to attach it if no itemAttachPoint set?
        }

        // Make item active in the hierarchy.
        item.gameObject.SetActive( true );
    }

    private void DisableItem( Item item )
    {
        // Sanity check.
        if( item == null ){ return; }

        // Set the item as a child of this game object and position.
        item.transform.SetParent( this.character.transform );
        item.transform.localPosition = Vector3.zero;

        // Hide the item in the hierarchy.
        item.gameObject.SetActive( false );
    }

    public Item GetActiveItem(){ return this.activeItem; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Script: TriggerGameObjects
    Author: Gareth Lockett
    Version: 1.0
    Description:    Script for activating or deactivating a game objects when a character enters or exits a trigger.
*/

// [ RequireComponent( typeof( Collider ) ) ]
public class TriggerGameObjects : Trigger
{
    // Enumerators
    public enum EventType{ _activate, _deactivate, _toggle }

    // Properties
    public EventType eventType;                 // Either activates, deactivates, or toggles active state.
    public GameObject[] gameObjectsToTrigger;   // An array of game objects to either activate or deactivate.

    // Methods
    protected override bool ActivateTrigger( Collider collider )
    {
        // Execute base/parent ActivateTrigger() first to check if this trigger should continue.
        if( base.ActivateTrigger( collider ) == false ){ return false; }
        
        // Do triggering based on event type.
        foreach( GameObject go in this.gameObjectsToTrigger )
        {
            // Check array elements are not empty (null)
            if( go == null ){ continue; }

            // Set active state based on event type.
            switch( this.eventType )
            {
                case EventType._activate: go.SetActive( true ); break; // Set active state to true (shown)

                case EventType._deactivate: go.SetActive( false ); break; // Set active state to false (hidden)

                case EventType._toggle: go.SetActive( !go.activeInHierarchy ); break; // Set to the opposite of its current active state.
            }
        }

        // Check to disable this script (eg will not trigger again unless enabled)
        if( this.disableOnTrigger == true ){ this.enabled = false; }

        return true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Script: TriggerInstantiate
    Author: Gareth Lockett
    Version: 1.0
    Description:    Script to instantiate a game object (eg prefab) when a character enters or exits a trigger.
                    Optional Transform for where to instantiate, otherwise will instantiate at same position/rotation as trigger.
                    Could be used to instantiate in enemies or items etc
*/

public class TriggerInstantiate : Trigger
{
    // Properties
    public GameObject gameObjectToInstantiate;          // Reference to the game object to instantiate (eg use a prefab)
    public Transform optionalLocationToInstantiate;     // Optional position/rotation to instantiate at (eg use empty game objects)
    public Vector3 instantiatePositionOffset;           // Extra option to modify the instantiated game object's position (eg instantiate next to the trigger or optional location position)

    // Methods
    protected override bool ActivateTrigger( Collider collider )
    {
        // Execute base/parent ActivateTrigger() first to check if this trigger should continue.
        if( base.ActivateTrigger( collider ) == false ){ return false; }

        // Sanity check.
        if( this.gameObjectToInstantiate == null ){ return false; }

        // Instantiate the game object.
        GameObject newGameObject = GameObject.Instantiate( this.gameObjectToInstantiate );

        // Fix Unity's stupid name change (eg "(Clone)")
        newGameObject.name = newGameObject.name.Replace( "(Clone)", "" );

        // Position/rotate the new game object.
        if( this.optionalLocationToInstantiate == null )
        {
            // Position/rotate at the trigger.
            newGameObject.transform.position = this.transform.position;
            newGameObject.transform.rotation = this.transform.rotation;
        }
        else
        {
            // Position/rotate at the optional location.
            newGameObject.transform.position = this.optionalLocationToInstantiate.position;
            newGameObject.transform.rotation = this.optionalLocationToInstantiate.rotation;
        }

        // Do the position offset.
        newGameObject.transform.position += this.instantiatePositionOffset;

        // Check to disable this script (eg will not trigger again unless enabled)
        if( this.disableOnTrigger == true ){ this.enabled = false; }

        return true;
    }
}

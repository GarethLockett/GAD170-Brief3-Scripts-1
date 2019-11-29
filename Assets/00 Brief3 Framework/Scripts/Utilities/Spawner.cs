using System.Collections.Generic;
using UnityEngine;

/*
    Script: Spawner
    Author: Gareth Lockett
    Version: 1.0
    Description: Simple spawning script. Selects and instantiates GameObjects at its current location/orientation at regular time intervals.
                 Enable/disable this component to control when it is spawning or not.
*/

public class Spawner : MonoBehaviour
{
    // Enumerators
    public enum SpawnType{ _random, _sequential }   // Type of spawning.

    // Properties
    public SpawnType spawnType;                     // Order in which the spawner will spawn objects.

    public List<GameObject> objectsToSpawn;         // The list of GameObjects to randomly select from and spawn (eg probably a reference to a PreFab)
    public bool removeWhenSpawned;                  // Should GameObject be removed from the objectsToSpawn list once instantiated?
    
    public float spawnTime = 4f;                    // Time between spawning objects.
    public bool selfDestructEmpty;                  // Destroy this spawner if no objects left to spawn.

    private float lastSpawnTime;                    // Last spawn time (eg next spawn time will be lastSpawnTime + spawnTime)
    private int lastSequentialId;                   // Keep track of the last id to spawn when in 'sequential' spawning state.

    // Methods
    //private void OnEnable(){ this.lastSpawnTime = Time.time; } // Reset the spawn time if this component gets re-enabled.

    private void Update()
    {
        // Sanity checks.
        if( this.objectsToSpawn == null ){ return; }                    // No list to spawn from (Should not happen?!)
        this.objectsToSpawn.RemoveAll( item => item == null );          // Remove any empty elements of the list.
        if( this.objectsToSpawn.Count == 0 )                            // No objects in the list to spawn.
        {
            if( this.selfDestructEmpty == true ){ Destroy( this.gameObject ); }
            return;
        }              

        if( Time.time < this.lastSpawnTime +this.spawnTime ){ return; } // Not time to spawn again yet.

        // Generate an element id from the list depending on the spawn type.
        int id = 0;
        switch( this.spawnType )
        {
            case SpawnType._random: // Randomly generate an element id from the list.
                id = Random.Range( 0, this.objectsToSpawn.Count );
                break;

            case SpawnType._sequential: // Get the next id from the list.
                // Check if the last sequential id is the last element of the object list, if so, loop back to start?
                if( this.lastSequentialId == this.objectsToSpawn.Count -1 ){ this.lastSequentialId = 0; }
                // If we are removing the objects from the list after they are spawned, then don't increment the last sequential id.
                else if( this.spawnType != SpawnType._sequential ){ this.lastSequentialId++; }

                id = this.lastSequentialId;
                break;
        }

        // Instantiate the object at this spawner location/orientation.
        GameObject newGameObject = GameObject.Instantiate( this.objectsToSpawn[ id ], this.transform.position, this.transform.rotation );

        // Fix Unity's stupid name change (eg "(Clone)")
        newGameObject.name = newGameObject.name.Replace( "(Clone)", "" );

        // If set, remove the spawned id from the object list.
        if( this.removeWhenSpawned == true ){ this.objectsToSpawn.RemoveAt( id ); }

        // Set the last spawn time.
        this.lastSpawnTime = Time.time;

// Debug.Log( "Spawner instantiated an object: " +go.name +"  ["+this.spawnType+"]", go );
    }
}

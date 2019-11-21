using UnityEngine;

/*
    Script: GameManagerPLATFORMER2D
    Author: Gareth Lockett
    Version: 1.0
    Description:    2D platformer game manager.
                    Checks if player has entered end BoxCollider (NOTE: BoxCollider should be world aligned, not at an angle!)
*/

public class GameManagerPLATFORMER2D : GameManager
{
    // Properties
    public BoxCollider endTrigger;      // Reference to the end trigger collider.

    private CharacterPlayer player;     // Reference to character player component in the scene.

    // Methods
    private void Start()
    {
        // Sanity check.
        if( this.endTrigger == null ){ Debug.LogWarning( "No end trigger set in GameManagerPLATFORMER2D?!", this.gameObject ); return; }

        // We are just using the end trigger BoxCollider as a volume to check if the player is within its' bounding box. Best to set it as a 'trigger' so doesn't interact with physics.
        this.endTrigger.isTrigger = true;

        // Find the CharacterPlayer component in the scene (eg should only be 1 for single player)
        this.player = GameObject.FindObjectOfType<CharacterPlayer>();
        if( this.player == null ){ Debug.LogWarning( "Could not find CharacterPlayer in GameManagerPLATFORMER2D!?", this.gameObject ); }
    }

    protected override void CheckGameState()
    {
        // Sanity check.
        if( this.endTrigger == null ){ Debug.LogWarning( "No end trigger set in GameManagerPLATFORMER2D?!", this.gameObject ); return; }
        if( this.player == null ){ Debug.LogWarning( "Could not find CharacterPlayer in GameManagerPLATFORMER2D!?", this.gameObject ); return; }
        
        // Check to see if the player position is within the end trigger bounding box.
        if( this.endTrigger.bounds.Contains( this.player.transform.position ) == true )
        {
            Debug.Log( "Player reached the end! 2D platformer game has ended." );

            // Set the game state as finished.
            this.SetGameState( GameState._finished );
        }
    }
}

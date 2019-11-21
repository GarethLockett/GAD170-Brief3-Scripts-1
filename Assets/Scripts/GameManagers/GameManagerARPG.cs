using UnityEngine;

/*
    Script: GameManagerARPG
    Author: Gareth Lockett
    Version: 1.0
    Description:    Action RPG game manager.
                    Checks if player has killed enemy boss.
*/

public class GameManagerARPG : GameManager
{
    // Properties
    public CharacterEnemy enemyBoss;     // Reference to the enemy boss in the scene (eg ARPG game type ends when this is null)

    // Methods
    private void Start()
    {
        // Subscribe to any PlayerControllerARPG objectClicked events.
        PlayerControllerARPG[] controllers = GameObject.FindObjectsOfType<PlayerControllerARPG>();
        foreach( PlayerControllerARPG controller in controllers )
        {
            controller.objectClicked += this.PlayerClickedAnObject;
//Debug.Log( "Subscribed to objectClicked event " +controller.gameObject.name );
        }
    }

    private void PlayerClickedAnObject( GameObject clickedGameObject )
    {
        // Sanity check.
        if( clickedGameObject == null ){ return; }

        // Check if object is on a Character.
        Character character = clickedGameObject.GetComponentInChildren<Character>();
        if( character == null ){ character = clickedGameObject.GetComponentInParent<Character>(); }
        if( character != null )
        {
            // Debug.Log( "Player clicked on a Character: " +character.name );

            // What to do when a player has clicked on a character (NOTE: Could be self!)

            // Check if it was an enemy that was clicked on.
            CharacterEnemy enemy = character as CharacterEnemy;
            if( enemy != null )
            {
                Debug.Log( "Player clicked on an Enemy: " +enemy.name );
            }

            return;
        }

        // Check if object is on an Item.
        Item item = clickedGameObject.GetComponentInChildren<Item>();
        if( item == null ){ item = clickedGameObject.GetComponentInParent<Item>(); }
        if( item != null )
        {
            Debug.Log( "Player clicked on an item: " +item.itemName );

            // Direct the player to head to the clicked item.
            CharacterPlayer player = GameManager.GetPlayer();
            if( player != null )
            {
                PlayerControllerARPG controller = player.GetComponent<PlayerControllerARPG>();
                if( controller != null ){ controller.SetNavMeshAgentDestination( item.transform.position ); }
            }

            return;
        }
        
        Debug.Log( "Player clicked on an object: " +clickedGameObject.name );
    }

    protected override void CheckGameState()
    {
        bool endGame = false;

        // Check if the enemy boss is missing (Presumed defeated and game object destroyed?)
        if( this.enemyBoss == null ){ endGame = true; }

        // Check if the game has ended (eg no more enemy boss)
        if( endGame == true )
        {
            Debug.Log( "Enemy boss defeated! ARPG game has ended." );

            // Set the game state as finished.
            this.SetGameState( GameState._finished );
        }
    }

}

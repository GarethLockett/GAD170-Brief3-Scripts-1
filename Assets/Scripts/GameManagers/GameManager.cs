using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : MonoBehaviour
{
    // Events
    public delegate void GameEnded();

    // Enumerators
    public enum GameState{ _starting, _running, _finished }

    // Properties
    public static GameEnded gameEnded;              // All characters, controllers, and items should subscribe to this delegate so when the game ends they can respond.

    private GameState state;                        // State of the current game. <<< 'public' for testing .. 'private' when done!
    private static List<CharacterPlayer> players;   // Player characters will register itself here (Probably just has 1 for now)

    // Methods
    private void Awake()
    {
        // Check there are no other GameManagers in the scene. Disable this one if others found!
        if( GameObject.FindObjectsOfType<GameManager>().Length > 1 )
        {
            Debug.LogWarning( "There are more than 1 GameManagers in the scene!", this.gameObject );
            this.gameObject.SetActive( false );
        }
        
        // Start game immediately.
        this.state = GameState._running;
    }

    // CharacterPlayers automatically register themselves here.
    public static void RegisterPlayer( CharacterPlayer player )
    {
        if( GameManager.players == null ){ GameManager.players = new List<CharacterPlayer>(); } // Make sure the list exists.
        if( GameManager.players.Contains( player ) == true ){ return; } // Don't add the player if they are already registered.
        GameManager.players.Add( player );
Debug.Log( "Registered player" );
    }

    public static CharacterPlayer GetPlayer()
    {
        // Sanity check.
        if( GameManager.players == null ){ return null; }
        if( GameManager.players.Count == 0 ){ return null; }

        // Return the first player.
        return GameManager.players[0];
    }

    public void SetGameState( GameState state )
    {
        // Log if there is a change of states.
        if( this.state != state )
        {
//Debug.Log( "Game state changing from " +this.state.ToString() +" to " +state.ToString() );

            // Set the new state.
            this.state = state;

            // Do state change.
            switch( this.state )
            {
                case GameState._starting: break;

                case GameState._running: break;

                case GameState._finished:
                    // Call any subscribed game ended methods (eg characters, controllers, items, projectiles etc)
                    if( GameManager.gameEnded != null ){ GameManager.gameEnded(); }
                    break;
            }
        }
    }

    private void Update()
    {
        // Do things here when game isn't running (eg check for start/restart?)

        // Only check game state after here if the game is running.
        if( this.state != GameState._running ){ return; }

        // Do current game state.
        switch( this.state )
        {
            case GameState._starting: break;

            case GameState._running:
                // Check game state. This will call the implemented CheckGameState() method on the inherited GameManager (eg for FPS, ARPG, Platformer etc)
                this.CheckGameState();

                break;

            case GameState._finished:
                break;
        }
    }

    // All GameManagers should implement this method (eg Check the win conditions for each type FPS, ARPG, Platformer etc)
    protected abstract void CheckGameState();
    
}

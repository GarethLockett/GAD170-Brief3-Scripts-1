using UnityEngine;

/*
    Script: GameManagerFPS
    Author: Gareth Lockett
    Version: 1.0
    Description:    First person shooter game manager.
                    Checks if time limit has run out yet.
*/

public class GameManagerFPS : GameManager
{
    // Properties
    public float timeLimit = 300f;      // Number of seconds the game will run for.

    private float gameTimeEnd;          // The time, in seconds, at which the game will end.

    // Methods
    private void Start()
    {
        // Set the game time end.
        this.gameTimeEnd = Time.time +this.timeLimit;
    }

    protected override void CheckGameState()
    {
        // Check the time limit.
        if( Time.time >= this.gameTimeEnd )
        {
            Debug.Log( "Time up! FPS game has ended." );
            
            // Set the game state as finished.
            this.SetGameState( GameState._finished );

            // Show the cursor in case it's still hidden.
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

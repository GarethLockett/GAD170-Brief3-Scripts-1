using UnityEngine;

/*
    Script: CharacterController
    Author: Gareth Lockett
    Version: 1.0
    Description:    Base class for all movement controllers (eg player, enemy)
*/

public abstract class CharacterController : MonoBehaviour
{
    // Properties

    // Methods
    private void Awake()
    {
        // Register for any GameManager events (Note this should work even if there are no active GameManagers in the scene)
        GameManager.gameEnded += this.GameEnded;
    }

    // Include this so can easily disable controllers (eg when character dies)
    private void Update(){}

    protected virtual void GameEnded()
    {
        // Fix in case this game object was just destroyed.
        if( this == null ){ return; }

        // Clean up any character related things when the game ends.
        //Debug.Log( "GameEnded called on Controller: " +this.gameObject.name );

        // Disable this character component at the end of the game so can't do anything else.
        this.enabled = false;
    }
}

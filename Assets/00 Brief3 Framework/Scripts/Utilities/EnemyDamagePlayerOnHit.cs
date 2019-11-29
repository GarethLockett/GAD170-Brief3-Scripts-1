using UnityEngine;

/*
    Script: EnemyDamagePlayerOnHit
    Author: Gareth Lockett
    Version: 1.0
    Description:    Script for having a character enemy deal some damage to a player character upon collision.
*/

[ RequireComponent( typeof( CharacterEnemy ) ) ]
public class EnemyDamagePlayerOnHit : MonoBehaviour
{
    // Properties
    public float amountOfDamageToDeal = 1f;         // Amount of damage to do to the player.
    public float timeBetweenDamage = 1f;            // Time between enemy dealing damage (eg so not every Update())

    private float lastDamageTime;                   // Last time this script damaged the player.

    // Methods
    private void OnCollisionStay( Collision collision )
    {
        // Check some time has passed since last damage.
        if( Time.time < this.lastDamageTime +this.timeBetweenDamage ){ return; }

        // Check the thing the enemy is colliding with is a CharacterPlayer
        CharacterPlayer player = collision.gameObject.GetComponent<CharacterPlayer>();
        if( player == null ){ return; }

        // Damage the player.
        player.TakeDamage( this.amountOfDamageToDeal );

        // Update the last damage time.
        this.lastDamageTime = Time.time;
    }
}

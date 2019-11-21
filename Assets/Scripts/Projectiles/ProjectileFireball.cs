using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Script: ProjectileFireball
    Author: Gareth Lockett
    Version: 1.0
    Description:    Fireball class.
                    Uses physics. Rigidbody & triggers for collisions (Make sure there is a collider on/(child of) this object)
*/

[ RequireComponent( typeof( Rigidbody ) ) ]
public class ProjectileFireball : Projectile
{
    // Properties

    // Methods
    private void Start()
    {
        // Make sure the rigidbody is set to isKinematic (eg NOT affected by gravity etc)
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    protected override void UpdateProjectile()
    {
        // Grab the current position (So can raycast for collisions)
        // Vector3 lastPosition = this.transform.position;

        // Update position. REMINDER: move speed is inherited from Projectile.
        this.transform.position += this.transform.forward *Time.deltaTime *this.moveSpeed;

/*
        // Raycast between last position and current position.
        RaycastHit hit;
        if( Physics.Raycast( lastPosition, this.transform.forward, out hit, ( this.transform.position -lastPosition ).magnitude ) == true )
        {
            // Position the projectile at the point of impact.
            this.transform.position = hit.point;

            // Check if the hit collider is a character and handle dealing damage.
            Character character = hit.collider.gameObject.GetComponent<Character>();
            if( character != null )
            {
                // Hit a character!
                character.ProjectileHit( this );
            }

            // Destroy the projectile object after it collides with something.
            GameObject.Destroy( this.gameObject );
        }
*/
    }

    private void OnTriggerEnter( Collider collider )
    {
        // Check to see if colliding with a Character.
        if( collider.attachedRigidbody != null )
        {
            Character character = collider.attachedRigidbody.gameObject.GetComponent<Character>();
            if( character != null )
            {
                // Don't do anything if this fireball is hitting the Character that instantiated it.
                if( character == this.sourceCharacter ){ return; }

                // Hit a character!
                // character.ProjectileHit( this );

                // Damage a character.
                character.TakeDamage( this.damage );
            }
        }

        // Destroy if it hits anything (Other than the Character that instantiated it)
        Destroy( this.gameObject );
    }

}

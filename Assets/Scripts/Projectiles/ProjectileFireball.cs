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
        // Update position. REMINDER: move speed is inherited from Projectile.
        this.transform.position += this.transform.forward *Time.deltaTime *this.moveSpeed;
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

                // Damage a character.
                character.TakeDamage( this.damage );
            }
 Debug.Log( "FIREBALL HIT: " +collider.gameObject.name, collider.gameObject );
        }


        // Destroy if it hits anything (Other than the Character that instantiated it)
        Destroy( this.gameObject );
    }

}

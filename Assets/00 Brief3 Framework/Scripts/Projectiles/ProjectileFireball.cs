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
        // Check not been triggered by the character (or part of) that fired the fireball.
        if( this.sourceCharacter != null )
        {
            if( collider.transform.IsChildOf( this.sourceCharacter.transform ) == true ){ return; }
        }

        // Check to see if colliding with a Character.
        if( collider.attachedRigidbody != null )
        {
            Character character = collider.attachedRigidbody.gameObject.GetComponent<Character>();
            if( character != null )
            {
                // Damage a character.
                character.TakeDamage( this.damage );
            }
 //Debug.Log( "FIREBALL HIT: " +collider.gameObject.name, collider.gameObject );
        }
        
        // Destroy if it hits anything.
        Destroy( this.gameObject );
    }

}

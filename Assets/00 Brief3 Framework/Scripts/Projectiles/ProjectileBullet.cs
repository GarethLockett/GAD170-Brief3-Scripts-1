using UnityEngine;

/*
    Script: ProjectileBullet
    Author: Gareth Lockett
    Version: 1.0
    Description:    Basic bullet class. Checks for and handles bullet hits.
*/

public class ProjectileBullet : Projectile
{
    // Properties

    // Methods
    protected override void UpdateProjectile()
    {
        // Grab the current position (So can raycast for collisions)
        Vector3 lastPosition = this.transform.position;

        // Update position. REMINDER: move speed is inherited from Projectile.
        this.transform.position += this.transform.forward *Time.deltaTime *this.moveSpeed;

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
                // Damage a character.
                character.TakeDamage( this.damage );
            }

            // Destroy the projectile object after it collides with something.
            GameObject.Destroy( this.gameObject );
        }
    }

}

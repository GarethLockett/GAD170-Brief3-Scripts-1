using UnityEngine;

/*
    Script: Projectile
    Author: Gareth Lockett
    Version: 1.0
    Description: Base projectile class that all projectiles inherit from. Handles projectile destruction (eg OnDestroy() )
*/

public abstract class Projectile : MonoBehaviour
{
    // Properties
    public float moveSpeed = 1f;                // Distance that the projectile moves per second.
    public float damage = 1f;                   // Amount of damage the projectile will do to a character.

    public AudioClip destroyedSound;            // Sound clip to play when the projectile gets destroyed.
    public ParticleSystem destroyedParticleFx;  // Particle effect to instantiate when the projectile gets destroyed.

    protected Character sourceCharacter;        // The character whos weapon instantiated the projectile (So can attribute hit to that character later)

    // Methods
    private void Awake()
    {
        // Register for any GameManager events (Note this should work even if there are no active GameManagers in the scene)
        GameManager.gameEnded += this.GameEnded;
    }

    protected virtual void GameEnded()
    {
        // Fix in case this game object was just destroyed.
        if( this == null ){ return; }

        // Clean up any character related things when the game ends.
//Debug.Log( "GameEnded called on Projectile: " +this.gameObject.name );

        // Destroy any projectiles at the end of the game?
        this.gameObject.SetActive( false ); // Disable first so doesn't trigger OnDestroy() method (eg sound)
        Destroy( this.gameObject );
    }

    private void Update()
    {
        this.UpdateProjectile();
    }

    public void SetSourceCharacter( Character source ){ this.sourceCharacter = source; }
    public Character GetSourceCharacter(){ return this.sourceCharacter; }

    protected virtual void UpdateProjectile(){}

    private void OnDestroy()
    {
        // Things to do when the projectile gets destroyed.
        
        // Play an audio clip when this projectile gets destroyed.
        // NOTE: Use AudioSource.PlayClipAtPoint to avoid audio not playing due to game object been destroyed.
        if( this.destroyedSound != null ){ AudioSource.PlayClipAtPoint( this.destroyedSound, this.transform.position, 1f ); }

        // Instantiate a particle effect when this projectile gets destroyed.
        // NOTE: Make sure particle system is set up to auto play and self destruct when finished playing.
        if( this.destroyedParticleFx != null )
        {
            GameObject.Instantiate( this.destroyedParticleFx.gameObject, this.transform.position, Quaternion.identity );
        }
    }
}

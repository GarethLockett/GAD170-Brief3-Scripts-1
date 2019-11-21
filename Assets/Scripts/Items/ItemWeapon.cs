using UnityEngine;

/*
    Script: ItemWeapon
    Author: Gareth Lockett
    Version: 1.0
    Description:    Generic weapon class. Actual weapon classes can inherit from this.
*/

public abstract class ItemWeapon : Item
{
    // Properties
    public GameObject projectilePrefab;     // Prefab to be instantiated when a shot is activated (NOTE: leave empty for none)
    public AudioClip equipSound;            // Sound that gets played when equiped by a character.

    private AudioSource audioFxSource;      // AudioSource for playing sounds on this weapon.

    // Methods
    protected virtual void Start()
    {
        // Auto add a collider (BoxCollider) so can trigger characters to pick it up.
        BoxCollider boxCollider = this.gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        // Try and get an existing audio source.
        this.audioFxSource = this.GetComponent<AudioSource>();

        // If a sound clip is already assigned then don't use (eg Might be set up for something else)
        if( this.audioFxSource != null ){ if( this.audioFxSource.clip != null ){ this.audioFxSource = null; } }

        // Create an audio source if no appropriate/empty ones found.
        if( this.audioFxSource == null )
            { this.audioFxSource = this.gameObject.AddComponent<AudioSource>(); this.audioFxSource.playOnAwake = false; }
    }

    public virtual void EquipWeapon()
    {
        // Play the equip sound.
        if( this.audioFxSource != null && this.equipSound != null )
        {
            this.audioFxSource.clip = this.equipSound;
            this.audioFxSource.Play();
        }
    }
}

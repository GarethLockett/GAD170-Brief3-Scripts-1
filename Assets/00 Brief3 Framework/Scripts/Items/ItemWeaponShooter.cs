using UnityEngine;

/*
    Script: ItemWeaponShooter
    Author: Gareth Lockett
    Version: 1.0
    Description: Basic weapon for shooting projectiles (Prefabs)
*/

public class ItemWeaponShooter : ItemWeapon
{
    // Properties
    public float rateOfFire = 0.25f;    // How much time, in seconds, between shots (eg lower = more shots)
    public int ammo = 12;               // Amount of ammunition. NOTE: -1 means infinite ammo.

    public AudioClip firingSound;       // Sound to play when firing this weapon.
    public AudioClip outOfAmmoSound;    // Sound to play if no more ammunition.

    private float nextFireTime;         // Don't fire anymore shots while Time.time is below this value.
    private AudioSource audioSource;    // The AudioSource auto-attached to this weapon at Start();

    // Methods
    protected override void Start()
    {
        // Call base class Start() for set up.
        base.Start();

        // Check if there is a firing or out of ammo assigned. If so, attach an AudioSource to the object.
        if( this.firingSound != null || this.outOfAmmoSound != null ){ this.audioSource = this.gameObject.AddComponent<AudioSource>(); }
    }

    public override void Activate( Character characterActivating )
    {
        // Check time against the next fire time.
        if( Time.time < this.nextFireTime ){ return; }

        // Set the next fire time (eg current time plus the rate of fire time amount)
        this.nextFireTime = Time.time +this.rateOfFire;

//Debug.Log( "ItemWeaponShooter fired! " +this.gameObject.name );

        // Check there is ammunition.
        if( this.ammo != -1 ) // -1 means infinite ammo.
        {
            if( this.ammo <= 0 )
            {
                if( this.outOfAmmoSound != null && this.audioSource.isPlaying == false )
                    { this.audioSource.clip = this.outOfAmmoSound; this.audioSource.Play(); }
                return;
            }
        }

        // Check if a projectile has been assigned, so can instantiate.
        if( this.projectilePrefab == null ){ return; }

        // Instantiate the projectile.
        GameObject projectileGO = GameObject.Instantiate( this.projectilePrefab, this.transform.position, this.transform.rotation );
        projectileGO.GetComponent<Projectile>().SetSourceCharacter( characterActivating );

        // De-increment the ammunition.
        this.ammo--;
    }
}

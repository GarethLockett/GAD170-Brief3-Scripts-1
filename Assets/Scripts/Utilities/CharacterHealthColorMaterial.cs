using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Script: CharacterDamageColorMaterial
    Author: Gareth Lockett
    Version: 1.0
    Description:    Script for flashing a Character a color when taking damage or gaining health.
                    Add this script to any game object with a MeshRenderer and material.
                    Uses watcher programming pattern.
*/

[ RequireComponent( typeof( MeshRenderer ) ) ]
public class CharacterHealthColorMaterial : MonoBehaviour
{
    // Properties
    public Character character;                 // Target character component to watch.
    public Color damageColor = Color.red;       // Color to flash to when taking damage.
    public Color healthColor = Color.green;     // Color to flash to when gaining health.
    public float fadeBackSpeed = 4f;            // Speed at which the color fades back to its original after flashing to a color (Higher values = faster)

    private Material mat;                       // Reference to this game object / mesh renderers' material (NOTE: Only supporting single material here)
    private float lastHealth;                   // Last character health check.
    private Color originalColor;                // Original material color at Start()

    // Methods
    private void Start()
    {
        // Get the material on this mesh renderer.
        this.mat = this.gameObject.GetComponent<MeshRenderer>().material;
        if( this.mat != null ){ this.originalColor = this.mat.color; }

        // Check a character has been assigned.
        if( this.character == null ){ Debug.LogWarning( "No character assigned to watch!?", this.gameObject ); return; }

        // Record the starting character health.
        this.lastHealth = this.character.GetHealth();

        // Record the starting material color.
        this.originalColor = this.mat.color;
    }

    private void Update()
    {
        // Sanity check.
        if( this.character == null ){ return; }
        if( this.mat == null ){ return; }

        // Check if the character health has changed since last check. If no change don't do anything else.
        float currentHealth = this.character.GetHealth();
        if( this.lastHealth == currentHealth ){ return; }

        // If health is less than last check (eg taken damage) then flash to damage color.
        if( currentHealth < this.lastHealth )
        {
            StopAllCoroutines(); // Stop any previous fades (eg avoid cross fading)
            StartCoroutine( this.FlashColorFadeBack( this.damageColor ) );
        }
        // Else flash to gaining health color.
        else
        {
            StopAllCoroutines(); // Stop any previous fades (eg avoid cross fading)
            StartCoroutine( this.FlashColorFadeBack( this.healthColor ) );
        }

        // Update the last health with the current health, ready for next check.
        this.lastHealth = currentHealth;
    }

    private IEnumerator FlashColorFadeBack( Color flashColor )
    {
        // Sanity check.
        if( this.mat == null ){ yield return null; }

        // Set the flash color.
        this.mat.color = flashColor;

        // Fade back from the flash color to the original color.
        while( this.mat.color != this.originalColor )
        {
            // Yield and wait until the end of the frame to loop again (eg stops this while loop from stalling Unity)
            yield return new WaitForEndOfFrame();

            // Fade a little bit for the amount of time that has passed (eg delta time)
            this.mat.color = Color.Lerp( this.mat.color, this.originalColor, Time.deltaTime *this.fadeBackSpeed );
        }
    }
}

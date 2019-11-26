using UnityEngine;

/*
    Script: TriggerSound
    Author: Gareth Lockett
    Version: 1.0
    Description:    Script for playing an audio clip (On an AudioSource) when a character enters or exits a trigger.
*/

public class TriggerSound : Trigger
{
    // Properties
    public AudioSource audioSource;         // The audio source to play.
    public AudioClip optionalAudioClip;     // Supplying a clip will assign it to the audio source before playing (eg so can have multiple triggers play different sounds from a single AudioSource)

    // Methods
    protected override bool ActivateTrigger( Collider collider )
    {
        // Execute base/parent ActivateTrigger() first to check if this trigger should continue.
        if( base.ActivateTrigger( collider ) == false ){ return false; }

        // Sanity check.
        if( this.audioSource == null ){ return false; }

        // If an optional audio clip is assigned then set AudioSource clip.
        if( this.optionalAudioClip != null ){ this.audioSource.clip = this.optionalAudioClip; }

        // Check audio source actually has a clip to play.
        if( this.audioSource.clip == null ){ return false; }

        // Play the audio source clip.
        this.audioSource.Play();

        // Check to disable this script (eg will not trigger again unless enabled)
        if( this.disableOnTrigger == true ){ this.enabled = false; }

        return true;
    }
}

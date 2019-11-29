using UnityEngine;

/*
    Script: TriggerAnimation
    Author: Gareth Lockett
    Version: 1.0
    Description:    Script for playing an animation clip (On an animation component) when a character enters or exits a trigger.
                    NOTE: This uses the older 'Legacy' animation system (eg Animation component NOT Mecanim and Animator component)
*/

public class TriggerAnimation : Trigger
{
    // Properties
    public Animation animationComponent;        // The Animation component.
    public string nameOfAnimationClip;          // This should already be added to the Animation component list of animations.

    // Methods
    protected override bool ActivateTrigger( Collider collider )
    {
        // Execute base/parent ActivateTrigger() first to check if this trigger should continue.
        if( base.ActivateTrigger( collider ) == false ){ return false; }

        // Try and get an animation state for the named clip.
        AnimationState aniState = this.animationComponent[ this.nameOfAnimationClip ];

        // Check if there is a valid animation name.
        if( string.IsNullOrEmpty( this.nameOfAnimationClip ) == true || aniState == null )
        {
            // If not, check if an animation clip is already assigned.
            if( this.animationComponent.clip != null )
            {
                if( this.animationComponent.isPlaying == false )
                {
                    // If there is animation clip, and not already playing, then play it.
                    this.animationComponent.Play( PlayMode.StopAll );
                    return true;
                }
            }
            return false;
        }

        // Try to set and play the named animation clip.
        if( aniState != null ){ this.animationComponent.clip = aniState.clip; }
        this.animationComponent.Play( PlayMode.StopAll );

        // Check to disable this script (eg will not trigger again unless enabled)
        if( this.disableOnTrigger == true ){ this.enabled = false; }

        return true;
    }
}

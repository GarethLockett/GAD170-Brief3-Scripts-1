using UnityEngine;

/*
    Script: Character
    Author: Gareth Lockett
    Version: 1.0
    Description:    Base class that all characters inherit from.
                    Tracks character states (eg idle, dead)
                    Handle character inventory.
*/

public abstract class Character : MonoBehaviour
{
    // Enumerators
    public enum CharacterState{ _idle, _dying, _dead }      // <<< Add more states here.

    // Properties
    public float health = 10f;                      // Amount of health the character has.

    public Inventory inventory;                     // This characters' inventory (Set to 'public' so designers can pre-add items to character inventory)

    protected CharacterState characterState;        // The current state of the character (eg idle, dead ...)

    public Transform itemAttachPoint;               // Reference to where items should be attached to when made active (eg hand in FPS)

    // Methods
    private void Awake()
    {
        // Register for any GameManager events (Note this should work even if there are no active GameManagers in the scene)
        GameManager.gameEnded += this.GameEnded;

        // Init inventory.
        this.inventory.Init( this );
    }

    protected virtual void GameEnded()
    {
        // Fix in case this game object was just destroyed.
        if( this == null ){ return; }

        // Clean up any character related things when the game ends.
//Debug.Log( "GameEnded called on Character: " +this.gameObject.name );

        // Disable this character component at the end of the game so can't do anything else.
        this.enabled = false;
    }

    public virtual void GetItem( Item item ){} // Gets called when a character somehow gets an item (eg picks up)

    public virtual void ActivateItem()
    {
        // Get the active item from the inventory.
        Item activeItem = this.inventory.GetActiveItem();

        // Check there is an active item to Activate()
        if( activeItem == null ){ return; }

        // Activate() the item.
        activeItem.Activate( this );
    }
    
    public virtual void TakeDamage( float amountOfDamage )
    {
        // Sanity check.
        if( amountOfDamage <= 0f ){ return; }

        // Substract the damage from this characters' health.
        this.health -= amountOfDamage;
    }

    // Allows other scripts to check the state of this character without been able to change it.
    public CharacterState GetState(){ return this.characterState; }

    // Check character health. Done after Update()
    private void LateUpdate()
    {
        // Check for player death (eg health <= 0)
        if( this.health <= 0f )
        {
            // TODO: What happens when players' health is equal to or less than 0?

            // Disable the characters' controller.
            CharacterController controller = this.gameObject.GetComponent<CharacterController>();
            if( controller != null ){ controller.enabled = false; }

            // Set this characters' state to dying (eg so a dying animation/effect can be played)
            this.characterState = CharacterState._dying;

            // TESTING: Just for now, Invoke the AutoDestroyWhenDead() method 3 seconds after death to destroy this game object.
            Invoke( "AutoDestroyWhenDead", 3f );

            // Disable this Character
            this.enabled = false;

//Debug.Log( "Character is dying!: " +this.gameObject.name );
            return;
        }

        return;
    }

    // Override trigger collisions in the inherited classes (eg for handling item pickups etc)
    protected virtual void OnTriggerEnter( Collider collider ){}

    private void AutoDestroyWhenDead()
    {
Debug.Log( "Character is dead!: " +this.gameObject.name );
        Destroy( this.gameObject );
    }
}
